using HotChocolatePOC.Data.Classes;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface IMutationExecuteService
    {
        Task<T> ExecuteInsertAsync<T>(MutationTemplate template) where T : class;
        Task<IEnumerable<T>> ExecuteInsertMultipleAsync<T>(MutationTemplate template) where T : class;
    }
}
