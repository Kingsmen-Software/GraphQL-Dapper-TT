using System.Reflection;
using System.Text;

using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;

namespace HotChocolatePOC.Data.Services
{
    public class MutationBuilderService : IMutationBuilderService
    {
        private readonly IDomainReflectionService _domainReflectionService;

        private const string _schemaName = "dbo";

        public MutationBuilderService(IDomainReflectionService domainReflectionService)
        {
            _domainReflectionService = domainReflectionService;
        }

        public MutationTemplate BuildMutationTemplate<T>(T entityToMutate) where T : class
        {
            Type type = typeof(T);

            return new MutationTemplate(
                BuildMutationSql(type),
                entityToMutate
            );
        }

        public MutationTemplate BuildMutationTemplate<T>(IEnumerable<T> entitiesToMutate) where T : class
        {
            Type type = typeof(T);

            //If is List<T> need to get the type from the generic arguement
            if (type.IsGenericType)
            {
                TypeInfo typeInfo = type.GetTypeInfo();
                bool implementsGenericIEnumerableOrIsGenericIEnumerable =
                    typeInfo.ImplementedInterfaces.Any(ti => ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>)) ||
                    typeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>);

                if (implementsGenericIEnumerableOrIsGenericIEnumerable)
                {
                    type = type.GetGenericArguments()[0];
                }
            }

            return new MutationTemplate(
                BuildMutationSql(type),
                entitiesToMutate
            );
        }

        private string BuildMutationSql(Type type)
        {
            //Either need to enforce a 1:1 with class name and table name or need to decorate classes with the table attribute
            string tableName = type.Name;
            StringBuilder columnList = new(null);
            StringBuilder parameterList = new(null);

            List<PropertyInfo> propertiesWithoutKey = _domainReflectionService.GetTypePropertiesWithoutKey(type);

            for (var i = 0; i < propertiesWithoutKey.Count; i++)
            {
                PropertyInfo property = propertiesWithoutKey[i];
                columnList.AppendFormat("[{0}]", property.Name);
                parameterList.AppendFormat("@{0}", property.Name);

                if (i < propertiesWithoutKey.Count - 1)
                {
                    columnList.Append(", ");
                    parameterList.Append(", ");
                }
            }

            return $"INSERT INTO [{_schemaName}].[{tableName}] ({columnList}) VALUES ({parameterList})";
        }
    }
}