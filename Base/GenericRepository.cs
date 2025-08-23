using Dapper;
using ms_evva_core.Base.Attributes;
using ms_evva_core.Repos.Interfaces;
using ms_evva_core.Utils;
using System.Data;
using System.Reflection;

namespace ms_evva_core.Base;

public class GenericRepository<T> : IRepository<T> where T : class
{
    protected string TableName { get; }
    protected readonly ConnectionProvider _connectionProvider = new();

    public GenericRepository(string tableName)
    {
        TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
    }

    protected string SelectColumns => string.Join(", ", typeof(T).GetProperties().Select(p => $"{GetColumnName(p)} AS {p.Name}"));

    protected virtual string GetColumnName(PropertyInfo property)
    {
        var columnAttribute = property.GetCustomAttribute<ColumnAttribute>();
        return columnAttribute?.Name ?? property.Name;
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        var sql = $"SELECT {SelectColumns} FROM {TableName} WHERE id = @Id";
        return await connection.QueryFirstOrDefaultAsync<T>(sql, new { Id = id });
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        return await connection.QueryAsync<T>($"SELECT {SelectColumns} FROM {TableName}");
    }

    protected virtual async Task<T?> FindFirstOrDefaultAsync(string whereClause, object? parameters = null)
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        var sql = $"SELECT {SelectColumns} FROM {TableName} WHERE {whereClause}";
        return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    public virtual async Task<int> AddAsync(T entity)
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name.ToLower() != "id");

        var columns = string.Join(", ", properties.Select(p => GetColumnName(p)));
        var parameters = string.Join(", ", properties.Select(p => $"@{p.Name}"));

        var sql = $"INSERT INTO {TableName} ({columns}) VALUES ({parameters}) RETURNING id";
        return await connection.ExecuteScalarAsync<int>(sql, entity);
    }

    public virtual async Task<bool> UpdateAsync(T entity)
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        var properties = typeof(T).GetProperties()
            .Where(p => p.Name.ToLower() != "id");

        var setClauses = properties.Select(p => $"{GetColumnName(p)} = @{p.Name}");
        var sql = $"UPDATE {TableName} SET {string.Join(", ", setClauses)} WHERE id = @Id";

        return await connection.ExecuteAsync(sql, entity) > 0;
    }

    public virtual async Task<bool> DeleteAsync(int id)
    {
        using var connection = await _connectionProvider.CreateConnectionAsync();
        var sql = $"DELETE FROM {TableName} WHERE id = @Id";
        return await connection.ExecuteAsync(sql, new { Id = id }) > 0;
    }
}