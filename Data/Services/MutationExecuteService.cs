using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;

namespace HotChocolatePOC.Data.Services
{
    public class MutationExecuteService : IMutationExecuteService
    {
        private readonly ISqlConnectionService _sqlConnectionService;

        public MutationExecuteService(ISqlConnectionService sqlConnectionService)
        {
            _sqlConnectionService = sqlConnectionService;
        }

        public async Task<T> ExecuteInsertAsync<T>(MutationTemplate template) where T : class
        {
            try
            {
                if (await _sqlConnectionService.InsertAsync<T>(template) == 0)
                {
                    return null;
                }

                return (T)template.EntityToMutate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<T>> ExecuteInsertMultipleAsync<T>(MutationTemplate template) where T : class
        {
            try
            {
                if (await _sqlConnectionService.InsertAsync<T>(template) == 0)
                {
                    return null;
                }

                return (IEnumerable<T>)template.EntityToMutate;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
