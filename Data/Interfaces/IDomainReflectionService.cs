using System.Reflection;

using HotChocolatePOC.Data.Classes;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface IDomainReflectionService
    {
        Type GetType(string entityName);
        TableJoin GetTableNameAndForeignKeyForJoin(string parentEntity, string entityName, JoinType joinType);
        List<PropertyInfo> GetTypePropertiesWithoutKey(Type type);
    }
}
