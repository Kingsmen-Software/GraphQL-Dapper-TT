using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;

namespace HotChocolatePOC.Data.Services
{
    public class DomainReflectionService : IDomainReflectionService
    {
        public Type GetType(string entityName)
        {
            return Type.GetType($"HotChocolatePOC.Domain.Entities.{entityName}, Domain");
        }

        public TableJoin GetTableNameAndForeignKeyForJoin(string parentEntity, string childEntity, JoinType joinType)
        {
            PropertyInfo[] properties = GetType(parentEntity).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                if (property.Name.ToLower() == childEntity.ToLower())
                {
                    Attribute? foreignKey = property.GetCustomAttribute(typeof(ForeignKeyAttribute));

                    //Means we have an ICollection that needs to populate
                    //Need to Recurse with the type of the ICollection instead to get the Foreign Key Id
                    if (foreignKey == null)
                    {
                        // return GetTableNameAndForeignKeyForOneToManyJoin(property.PropertyType.GetGenericArguments().First().Name, parentEntity);
                    }

                    if (property.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        return GetTableNameAndForeignKeyForOneToManyJoin(property.PropertyType.GenericTypeArguments.First().Name, parentEntity);
                    }

                    //If we left joined on the prior join we need to continue the left join for the rest of the related entity joins
                    return new TableJoin()
                    {
                        ForeignKeyName = (foreignKey as ForeignKeyAttribute).Name,
                        EntityToJoin = property.PropertyType.Name,
                        EntityWithForeignKey = property.ReflectedType.Name,
                        TopLevelEntity = property.PropertyType.Name,
                        JoinType = joinType,
                    };
                }
            }

            return null;
        }

        private TableJoin GetTableNameAndForeignKeyForOneToManyJoin(string parentEntity, string childEntity)
        {
            PropertyInfo[] properties = GetType(parentEntity).GetProperties();
            Type childType = GetType(childEntity);

            foreach (PropertyInfo property in properties)
            {
                if (property.PropertyType == GetType(childEntity))
                {
                    Attribute? foreignKey = property.GetCustomAttribute(typeof(ForeignKeyAttribute));

                    TableJoin result = new TableJoin()
                    {
                        ForeignKeyName = (foreignKey as ForeignKeyAttribute).Name,
                        EntityWithForeignKey = property.ReflectedType.Name,
                        EntityToJoin = property.PropertyType.Name,
                        JoinType = JoinType.Left,
                        TopLevelEntity = property.ReflectedType.Name
                    };

                    return result;
                }
            }

            return null;
        }

        //We need to remove any Key columns, ICollections, and any base properties so that we just have
        //The list of Columns we need to insert against
        public List<PropertyInfo> GetTypePropertiesWithoutKey(Type type)
        {
            return type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public)
                .Where(prop => !Attribute.IsDefined(prop, typeof(KeyAttribute))
                    && !Attribute.IsDefined(prop, typeof(ForeignKeyAttribute))
                    && !(prop.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(prop.PropertyType)))
                .ToList();
        }
    }
}
