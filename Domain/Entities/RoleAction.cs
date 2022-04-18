using HotChocolatePOC.Domain.Classes;

namespace HotChocolatePOC.Domain.Entities
{
    public class RoleAction : Entity
    {
        public bool? CanRead { get; set; }
        public bool? CanWrite { get; set; }
    }
}
