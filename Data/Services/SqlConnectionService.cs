using System.Data.SqlClient;

using HotChocolatePOC.Data.Interfaces;
using HotChocolatePOC.Domain.Interfaces;
using HotChocolatePOC.Data.Classes;

using Dapper;

namespace HotChocolatePOC.Data.Services
{
    public class SqlConnectionService : ISqlConnectionService
    {
        private readonly IContextData _contextData;
        public SqlConnectionService(IContextData contextData)
        {
            _contextData = contextData;
        }

        public SqlConnection GetSqlConnection() => new(_contextData.DatabaseConnectionString);

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            using (var db = GetSqlConnection())
            {
                return await db.QueryAsync<T>(sql);
            }
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters)
        {
            using (var db = GetSqlConnection())
            {
                return await db.QueryAsync<T>(sql, parameters);
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sql, object parameters)
        {
            using (var db = GetSqlConnection())
            {
                return await db.QuerySingleAsync<T>(sql, parameters);
            }
        }

        public async Task<int> InsertAsync<T>(MutationTemplate mutationTemplate) where T : class
        {
            using (var db = GetSqlConnection())
            {
                return await db.ExecuteAsync(mutationTemplate.RawSql, mutationTemplate.EntityToMutate);
            }
        }
    }
}