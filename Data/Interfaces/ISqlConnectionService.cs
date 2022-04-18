namespace HotChocolatePOC.Data.Interfaces
{
    public interface ISqlConnectionService
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
        Task<T> QuerySingleAsync<T>(string sql, object parameters);
    }
}
