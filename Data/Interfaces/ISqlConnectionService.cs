using HotChocolatePOC.Data.Classes;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface ISqlConnectionService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters);
        Task<T> QuerySingleAsync<T>(string sql, object parameters);
        Task<int> InsertAsync<T>(MutationTemplate mutationTemplate) where T : class;
    }
}