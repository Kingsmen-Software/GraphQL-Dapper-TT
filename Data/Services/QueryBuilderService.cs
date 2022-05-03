using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;

using HotChocolate.Language;

using Dapper;

using static Dapper.SqlBuilder;

namespace HotChocolatePOC.Data.Services
{
    public class QueryBuilderService : IQueryBuilderService
    {
        private readonly IDomainReflectionService _domainReflectionService;

        private const string _schemaName = "dbo";

        private string _fromTable = string.Empty;

        private SqlBuilder _query = new SqlBuilder();
        private DynamicParameters _dynamicParameters = new DynamicParameters();

        public QueryBuilderService(IDomainReflectionService domainReflectionService)
        {
            _domainReflectionService = domainReflectionService;
        }

        public async Task<QueryTemplate> BuildQueryTemplateAsync(SelectionSetNode selectionSet)
        {
            try
            {
                BuildQuery(selectionSet);

                return GetSqlBuilderTemplate();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<QueryTemplate> BuildQueryTemplateAsync(SelectionSetNode selectionSet, Guid? id)
        {
            try
            {
                BuildQuery(selectionSet);

                CreateWhere(_fromTable, "id", "id", id);

                return GetSqlBuilderTemplate();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private bool BuildQuery(SelectionSetNode selectionSet)
        {
            List<string> selectColumns = new List<string>();

            foreach (ISelectionNode selection in selectionSet.Selections)
            {
                Type selectionType = selection.GetType();

                if (selectionType == typeof(InlineFragmentNode))
                {
                    InlineFragmentNode selectionCast = selection as InlineFragmentNode;

                    if (string.IsNullOrEmpty(_fromTable))
                    {
                        _fromTable = selectionCast?.TypeCondition.Name.Value;

                        //Get the fragment unique select columns
                        foreach (ISelectionNode item in selectionCast.SelectionSet.Selections.Where(x => (x as FieldNode).SelectionSet == null))
                        {
                            if (item != null)
                            {
                                selectColumns.Add((item as FieldNode).Name.Value);
                            }
                        }

                        selectColumns.Add("id");
                        CreateSelect(selectColumns, _fromTable);
                    }

                    //Build Query for linked entities
                    //And mark that we have a join so we use the right template
                    foreach (ISelectionNode item in selectionCast.SelectionSet.Selections.Where(x => (x as FieldNode).SelectionSet != null))
                    {
                        FieldNode itemCast = item as FieldNode;

                        //Need to set the base alias for the nested join
                        //Each recursion of BuildJoinQuery will append to this
                        var aliasName = $"{itemCast.Name.Value}_";
                        BuildJoinQuery(item as FieldNode, _fromTable, selectPrefix: aliasName);
                    }
                }
            }

            return true;
        }

        private bool BuildJoinQuery(FieldNode? fieldNode, string parentEntity, string selectPrefix, JoinType joinType = JoinType.Inner)
        {
            if (fieldNode == null)
            {
                //do something here
                return false;
            }

            List<string> selectColumns = new List<string>();

            //fieldNode.Name.Value is the name of the join object on the entity.
            //This does not have to be 1 to 1 with the DB table
            //We use DomainReflectionService to grab the actual DB name off this
            //and to get the ForeignKeyAttribute we will use to create the join
            TableJoin joinInfo = _domainReflectionService.GetTableNameAndForeignKeyForJoin(parentEntity, fieldNode.Name.Value, joinType);

            CreateJoin(joinInfo);

            //Create the select statements unique to the join 
            foreach (ISelectionNode selection in fieldNode.SelectionSet.Selections.Where(x => (x as FieldNode).SelectionSet == null))
            {
                selectColumns.Add((selection as FieldNode).Name.Value);
            }

            selectColumns.Add("id");
            CreateSelect(selectColumns, joinInfo.TopLevelEntity, selectPrefix);

            //If we have further joins we use recursion to continue to populate
            foreach (ISelectionNode selection in fieldNode.SelectionSet.Selections.Where(x => (x as FieldNode).SelectionSet != null))
            {
                FieldNode selectionCast = selection as FieldNode;

                //Need to add onto the select alias name for each instance of recurison so we can map the columns returned back to a nested POCO
                var newPrefix = selectPrefix + $"{selectionCast.Name.Value}_";

                BuildJoinQuery(selectionCast, joinInfo.TopLevelEntity, newPrefix, joinInfo.JoinType);
            }

            return true;
        }

        private void CreateSelect(List<string> fieldsToAdd, string tableName, string? tablePrefix = null)
        {
            var typeProps = _domainReflectionService.GetType(tableName).GetProperties().Select(t => t.Name.ToLower());

            //slapper automapper requires underscore notation for select alias in order to map correctly to nest entities
            foreach (string field in fieldsToAdd.Distinct().Where(f => typeProps.Contains(f.ToLower())))
            {
                _query.Select($"[{tableName}].[{field}] AS [{tablePrefix ?? string.Empty}{field}]");
            }
        }

        private void CreateJoin(TableJoin tableJoin)
        {
            string joinStatement = $"[{_schemaName}].[{tableJoin.TopLevelEntity}] AS [{tableJoin.TopLevelEntity}] ON [{tableJoin.EntityWithForeignKey}].[{tableJoin.ForeignKeyName}] = [{tableJoin.EntityToJoin}].[Id]";

            if (tableJoin.JoinType == JoinType.Left)
            {
                _query.LeftJoin(joinStatement);
            }
            else
            {
                _query.InnerJoin(joinStatement);
            }
        }

        private void CreateWhere<T>(string fromTable, string column, string filterName, T value)
        {
            _query.Where($"[{fromTable}].[{column}] = @{filterName}");
            _dynamicParameters.Add($"@{filterName}", value);
        }

        private QueryTemplate GetSqlBuilderTemplate()
        {
            Template template = _query.AddTemplate($"SELECT /**select**/ FROM [{_schemaName}].[{_fromTable}] /**innerjoin**/ /**leftjoin**/ /**where**/ ");

            return new QueryTemplate(
                template.RawSql,
                _dynamicParameters,
                _fromTable
            );
        }
    }
}
