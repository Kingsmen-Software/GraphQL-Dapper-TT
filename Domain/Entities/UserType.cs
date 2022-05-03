using HotChocolatePOC.Domain.Classes;

namespace HotChocolatePOC.Domain.Entities
{
    public class UserType : Entity
    {
        public string Type { get; set; } = string.Empty;

        //Slapper Automapper needs a parameterless constructor
        public UserType() { }
    }
}
