using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace datos.interfaz
{
    /// <summary>
    /// Interfaz para realizar el llamado a base de datos y ejecución de consultas SQL
    /// </summary>
    public interface IDapperConnector
    {
        Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);

        Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
