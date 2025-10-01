using System.Data;
using Dapper;

namespace ms_evva_core.Base.TypeHandlers;

public class PostgresEnumHandler<T> : SqlMapper.ITypeHandler where T : Enum
{
    public void SetValue(IDbDataParameter parameter, object value)
    {
        if (value == null)
        {
            parameter.Value = DBNull.Value;
        }
        else
        {
            parameter.Value = value.ToString()?.ToLower();
        }
    }

    public object Parse(Type destinationType, object value)
    {
        return value != null ? Enum.Parse(destinationType, value.ToString()!, true) : null!;
    }
}
