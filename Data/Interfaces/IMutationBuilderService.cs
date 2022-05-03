using HotChocolatePOC.Data.Classes;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface IMutationBuilderService
    {
        MutationTemplate BuildMutationTemplate<T>(T entityToMutate) where T : class;
        MutationTemplate BuildMutationTemplate<T>(IEnumerable<T> entitiesToMutate) where T : class;
    }
}
