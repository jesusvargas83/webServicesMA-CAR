using Dapper;
using datos.interfaz;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace datos.implementacion
{
    /// <summary>
    /// Clase que implementa el llamado a base de datos y ejecución de consultas SQL
    /// </summary>
    public class DapperConnector : IDapperConnector
    {
        private const string ERRORMESSAGE = "Error durante llamado a base de datos en método {0}.";

        private readonly string LocalPostgresConnectionString;
        private readonly string AzurePostgresConnectionString;
        private readonly bool UseLocalPostgresDB;

        private readonly ILogger<DapperConnector> logger;

        /// <summary>
        /// Constructor de la clase que recibe la inyección de dependencias
        /// </summary>
        /// <param name="configuration">Objeto de configuración de la solución</param>
        /// <param name="logger">Objeto para realizar el logging</param>
        public DapperConnector(IConfiguration configuration, ILogger<DapperConnector> logger)
        {
            this.LocalPostgresConnectionString = configuration.GetConnectionString("LocalPostgresConnection");
            this.AzurePostgresConnectionString = configuration.GetConnectionString("AzurePostgresConnection");
            bool.TryParse(configuration.GetConnectionString("UseLocalPostgresDB"), out UseLocalPostgresDB);

            this.logger = logger;
        }

        public async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string DbConnectionString = UseLocalPostgresDB ? LocalPostgresConnectionString : AzurePostgresConnectionString;
            using var connection = new NpgsqlConnection(DbConnectionString);

            try
            {
                connection.Open();

                var result = await connection.QueryAsync<T>(sql, param, transaction, commandTimeout, commandType);
                return result.AsList();
            }
            catch (Exception exception)
            {
                logger.LogError(string.Format(ERRORMESSAGE, nameof(QueryAsync)), exception);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public async Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            string DbConnectionString = UseLocalPostgresDB ? LocalPostgresConnectionString : AzurePostgresConnectionString;
            using var connection = new NpgsqlConnection(DbConnectionString);

            try
            {
                connection.Open();

                var result = await connection.QuerySingleOrDefaultAsync<T>(sql, param, transaction, commandTimeout, commandType);
                return result;
            }
            catch (Exception exception)
            {
                logger.LogError(string.Format(ERRORMESSAGE,nameof(QuerySingleOrDefaultAsync)), exception);
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
