namespace HotChocolatePOC.Domain.MutationTypes.InputExtensions
{
    public static class InputExtensions
    {
        public static IEnumerable<TOutput> ToClass<TOutput, TInput>(this IEnumerable<TInput> inputs)
        {
            return inputs.Select(x => (TOutput)Activator.CreateInstance(typeof(TOutput), x));
        }
    }
}
