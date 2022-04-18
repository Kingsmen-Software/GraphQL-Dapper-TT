using HotChocolatePOC.Domain.Interfaces;

namespace HotChocolatePOC.Domain.Classes
{
    public class ContextData : IContextData
    {
        public string DatabaseConnectionString { get; set; } = string.Empty;
    }
}
