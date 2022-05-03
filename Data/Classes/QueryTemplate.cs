namespace HotChocolatePOC.Data.Classes
{
    public record QueryTemplate(
        string RawSql,
        object Parameters,
        string TopLevelEntity
    );
}
