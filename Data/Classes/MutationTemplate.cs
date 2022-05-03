namespace HotChocolatePOC.Data.Classes
{
    public record MutationTemplate(
        string RawSql,
        object EntityToMutate
    );
}
