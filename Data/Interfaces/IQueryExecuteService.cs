using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Domain.Classes;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface IQueryExecuteService
    {
        Task<IEnumerable<Entity>> ExecuteQueryAsync(QueryTemplate template);
        Task<Entity> ExecuteQuerySingleAsync(QueryTemplate template);
    }
}
