using Dapper;
using ms_evva_core.Base.Attributes;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;
using NpgsqlTypes;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ms_evva_core.Base;

/// <summary>
/// Generic repository implementation with basic CRUD operations support
/// </summary>
/// <typeparam name="T">Entity type that this repository manages</typeparam>
public class GenericRepository<T> : IRepository<T> where T : class
{
    protected string TableName { get; }
    protected readonly ConnectionProvider _connectionProvider;
    protected readonly PropertyInfo[] _entityProperties;
    protected readonly PropertyInfo? _idProperty;

    public GenericRepository(string tableName)
    {
        TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
        _connectionProvider = new ConnectionProvider();
        _entityProperties = typeof(T).GetProperties();
        _idProperty = _entityProperties.FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the column mapping for SELECT queries
    /// </summary>
    protected virtual string SelectColumns => string.Join(", ", 
        _entityProperties.Select(p => $"{GetColumnName(p)} AS {p.Name}"));

    /// <summary>
    /// Gets the database column name for a property
    /// </summary>
    /// <param name="property">Property information from reflection</param>
    /// <returns>The corresponding database column name</returns>
    protected virtual string GetColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
        return columnAttribute?.Name ?? PascalToSnakeCase(property.Name);
    }

    private string PascalToSnakeCase(string text)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return Regex.Replace(text, "(?<!^)([A-Z])", "_$1").ToLower();
    }

    private bool IsEnum(PropertyInfo prop, out Type? enumType)
    {
        enumType = null;
        if (prop.PropertyType.IsEnum)
        {
            enumType = prop.PropertyType;
            return true;
        }
        var underlyingType = Nullable.GetUnderlyingType(prop.PropertyType);
        if (underlyingType?.IsEnum ?? false)
        {
            enumType = underlyingType;
            return true;
        }
        return false;
    }

    protected virtual string GetEnumTypeName(PropertyInfo property)
    {
        if (IsEnum(property, out var enumType) && enumType != null)
        {
            var pgNameAttribute = enumType.GetCustomAttribute<PgNameAttribute>();
            return pgNameAttribute?.PgName ?? PascalToSnakeCase(enumType.Name);
        }
        throw new InvalidOperationException($"Property {property.Name} is not an enum type.");
    }

    /// <summary>
    /// Creates query parameters handling enum types appropriately
    /// </summary>
    /// <param name="entity">Entity instance to extract parameters from</param>
    /// <param name="includeId">Whether to include the Id property in parameters</param>
    /// <returns>DynamicParameters object ready for use with Dapper</returns>
    protected virtual DynamicParameters CreateParameters(T entity, bool includeId = false)
    {
        var parameters = new DynamicParameters();
        var properties = _entityProperties.Where(p => includeId || !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
        
        foreach (var prop in properties)
        {
            var value = prop.GetValue(entity);
            if (value != null && IsEnum(prop, out _))
            {
                parameters.Add(prop.Name, value.ToString()?.ToLower(), DbType.String);
            }
            else
            {
                parameters.Add(prop.Name, value);
            }
        }

        return parameters;
    }

    /// <summary>
    /// Retrieves an entity by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve</param>
    /// <returns>The entity if found, null otherwise</returns>
    public virtual async Task<T?> GetByIdAsync(int id)
    {
        const string SQL_TEMPLATE = "SELECT {0} FROM {1} WHERE id = @Id";
        using var connection = await _connectionProvider.CreateConnectionAsync();
        
        return await connection.QueryFirstOrDefaultAsync<T>(
            string.Format(SQL_TEMPLATE, SelectColumns, TableName),
            new { Id = id });
    }

    /// <summary>
    /// Retrieves all entities from the table
    /// </summary>
    /// <returns>A collection of all entities</returns>
    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        const string SQL_TEMPLATE = "SELECT {0} FROM {1}";
        using var connection = await _connectionProvider.CreateConnectionAsync();
        
        return await connection.QueryAsync<T>(
            string.Format(SQL_TEMPLATE, SelectColumns, TableName));
    }

    /// <summary>
    /// Finds the first entity matching the specified where clause
    /// </summary>
    /// <param name="whereClause">SQL where clause</param>
    /// <param name="parameters">Query parameters</param>
    /// <returns>The first matching entity or null if none found</returns>
    protected virtual async Task<T?> FindFirstOrDefaultAsync(string whereClause, object? parameters = null)
    {
        const string SQL_TEMPLATE = "SELECT {0} FROM {1} WHERE {2}";
        using var connection = await _connectionProvider.CreateConnectionAsync();
        
        return await connection.QueryFirstOrDefaultAsync<T>(
            string.Format(SQL_TEMPLATE, SelectColumns, TableName, whereClause),
            parameters);
    }

    /// <summary>
    /// Adds a new entity to the database
    /// </summary>
    /// <param name="entity">The entity to add</param>
    /// <returns>The ID of the newly created entity</returns>
    public virtual async Task<int> AddAsync(T entity)
    {
        var properties = _entityProperties.Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
        var columns = properties.Select(p => GetColumnName(p));
        var parameters = properties.Select(p => 
            IsEnum(p, out _) ? $"@{p.Name}::{GetEnumTypeName(p)}" : $"@{p.Name}");

        const string SQL_TEMPLATE = "INSERT INTO {0} ({1}) VALUES ({2}) RETURNING id";
        var sql = string.Format(SQL_TEMPLATE,
            TableName,
            string.Join(", ", columns),
            string.Join(", ", parameters));

        using var connection = await _connectionProvider.CreateConnectionAsync();
        return await connection.ExecuteScalarAsync<int>(sql, CreateParameters(entity));
    }

    /// <summary>
    /// Updates an existing entity in the database
    /// </summary>
    /// <param name="entity">The entity with updated values</param>
    /// <returns>True if the entity was updated, false otherwise</returns>
    public virtual async Task<bool> UpdateAsync(T entity)
    {
        if (_idProperty == null) return false;

        var properties = _entityProperties.Where(p => !p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));
        var setClauses = properties.Select(p => 
            IsEnum(p, out _) ? $"{GetColumnName(p)} = @{p.Name}::{GetEnumTypeName(p)}" : $"{GetColumnName(p)} = @{p.Name}");

        const string SQL_TEMPLATE = "UPDATE {0} SET {1} WHERE id = @Id";
        var sql = string.Format(SQL_TEMPLATE,
            TableName,
            string.Join(", ", setClauses));

        using var connection = await _connectionProvider.CreateConnectionAsync();
        return await connection.ExecuteAsync(sql, CreateParameters(entity, true)) > 0;
    }

    /// <summary>
    /// Deletes an entity from the database by its ID
    /// </summary>
    /// <param name="id">The ID of the entity to delete</param>
    /// <returns>True if the entity was deleted, false if it wasn't found</returns>
    public virtual async Task<bool> DeleteAsync(int id)
    {
        const string SQL_TEMPLATE = "DELETE FROM {0} WHERE id = @Id";
        using var connection = await _connectionProvider.CreateConnectionAsync();
        
        return await connection.ExecuteAsync(
            string.Format(SQL_TEMPLATE, TableName),
            new { Id = id }) > 0;
    }
}