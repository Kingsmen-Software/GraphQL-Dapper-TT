using System.ComponentModel.DataAnnotations.Schema;

using HotChocolatePOC.Domain.Classes;

namespace HotChocolatePOC.Domain.Entities
{
    public class User : Entity
    { 
        public string? UserName { get; set; }

        public Guid? UserType_Id { get; set; }
        [ForeignKey("UserType_Id")]
        public UserType? UserType { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
