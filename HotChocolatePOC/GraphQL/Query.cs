using HotChocolatePOC.Data.Interfaces;
using HotChocolatePOC.Domain.Classes;
using HotChocolatePOC.Data.Classes;

using HotChocolate.Resolvers;
using HotChocolate.Execution.Processing;

namespace HotChocolatePOC.GraphQL.Query
{
    public class Query
    {
        public async Task<IEnumerable<Entity>> EntityGetAll(
            IResolverContext context,
            [Service] IQueryBuilderService builder,
            [Service] IQueryExecuteService executor
        )
        {
            try
            {
                var topLevelSelection = context.Selection as Selection;
                var selectionSet = topLevelSelection.SelectionSet;

                QueryTemplate template = await builder.BuildQueryTemplateAsync(selectionSet);

                return await executor.ExecuteQueryAsync(template);
            }
            catch (Exception ex)
            {
                //handle exception however you need to
                return null;
            }
        }

        public async Task<Entity> EntityGetById(
            IResolverContext context,
            [Service] IQueryBuilderService builder,
            [Service] IQueryExecuteService executor,
            Guid? id
        )
        {
            try
            {
                var topLevelSelection = context.Selection as Selection;
                var selectionSet = topLevelSelection.SelectionSet;

                QueryTemplate template = await builder.BuildQueryTemplateAsync(selectionSet, id);

                return await executor.ExecuteQuerySingleAsync(template);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
