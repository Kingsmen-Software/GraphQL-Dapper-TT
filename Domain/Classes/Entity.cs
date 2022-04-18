using HotChocolatePOC.Domain.Interfaces;

using HotChocolate.Types;

namespace HotChocolatePOC.Domain.Classes
{
    [InterfaceType]
    public abstract class Entity : IEntity
    {
        public Guid Id { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? UpdateDateTime { get; set; }
    }
}
