namespace HotChocolatePOC.Data.Classes
{
    public class TableJoin
    {
        public string TopLevelEntity { get; set; } = string.Empty;
        public string EntityWithForeignKey { get; set; } = string.Empty;
        public string EntityToJoin { get; set; } = string.Empty;
        public string ForeignKeyName { get; set; } = string.Empty;
        public JoinType JoinType { get; set; }
    }

    public enum JoinType
    {
        Inner = 0,
        Left = 1,
    }
}
