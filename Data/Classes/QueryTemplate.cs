namespace HotChocolatePOC.Data.Classes
{
    public class QueryTemplate
    {
        public string RawSql { get; set; }
        public object Parameters { get; set; }
        public string TopLevelEntity { get; set; }
    }
}
