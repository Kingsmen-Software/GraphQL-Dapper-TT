namespace HotChocolatePOC.Domain.Interfaces
{
    public interface IEntity
    {
        Guid Id { get; set; }
        bool? IsActive { get; set; }
        bool? IsDeleted { get; set; }
        DateTime? CreateDateTime { get; set; }
        DateTime? UpdateDateTime { get; set; }
    }
}
