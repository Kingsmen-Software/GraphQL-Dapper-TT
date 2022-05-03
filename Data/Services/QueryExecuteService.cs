using System.Reflection;

using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;
using HotChocolatePOC.Domain.Classes;

using Slapper;

namespace HotChocolatePOC.Data.Services
{
    public class QueryExecuteService : IQueryExecuteService
    {
        private readonly ISqlConnectionService _sqlConnectionService;
        private readonly IDomainReflectionService _domainReflectionService;

        public QueryExecuteService(
            ISqlConnectionService sqlConnectionService,
            IDomainReflectionService domainReflectionService
            )
        {
            _sqlConnectionService = sqlConnectionService;
            _domainReflectionService = domainReflectionService;
        }

        public async Task<IEnumerable<Entity>> ExecuteQueryAsync(QueryTemplate template)
        {
            try
            {
                dynamic queryResult = await _sqlConnectionService.QueryAsync<dynamic>(template.RawSql);

                return MapResult(queryResult, template.TopLevelEntity, true) as IEnumerable<Entity>;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Entity> ExecuteQuerySingleAsync(QueryTemplate template)
        {
            try
            {
                dynamic queryResult = await _sqlConnectionService.QueryAsync<dynamic>(template.RawSql, template.Parameters);

                IEnumerable<Entity> mappedResult = MapResult(queryResult, template.TopLevelEntity, true) as IEnumerable<Entity>;

                Entity result = mappedResult.FirstOrDefault();

                if (result == null)
                {
                    return null;
                }

                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private object? MapResult(dynamic queryResult, string topLevelEntity, bool isEnumerable = false)
        {
            //Need to tell slapper automapper which column and table to look for on the flat list in order
            //to correctly return a mapped list. This is the top level entity
            AutoMapper.Configuration.AddIdentifier(_domainReflectionService.GetType(topLevelEntity), "id");

            MethodInfo? method = null;

            if (isEnumerable)
            {
                method = typeof(AutoMapper).GetMethod(nameof(AutoMapper.MapDynamic), new[] { typeof(IEnumerable<object>), typeof(bool) });
            }
            else
            {
                method = typeof(AutoMapper).GetMethod(nameof(AutoMapper.MapDynamic), new[] { typeof(object), typeof(bool) });
            }

            if (method == null)
            {
                return null;
            }

            MethodInfo generic = method.MakeGenericMethod(_domainReflectionService.GetType(topLevelEntity));
            return generic.Invoke(null, new object[] { queryResult, true });
        }
    }
}
