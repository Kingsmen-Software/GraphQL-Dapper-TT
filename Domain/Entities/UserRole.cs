using System.ComponentModel.DataAnnotations.Schema;

using HotChocolatePOC.Domain.Classes;

namespace HotChocolatePOC.Domain.Entities
{
    public class UserRole : Entity
    {
        public string? RoleName { get; set; }

        public Guid? User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User? User { get; set; }

        public Guid? RoleAction_Id { get; set; }
        [ForeignKey("RoleAction_Id")]
        public RoleAction? RoleAction { get; set; }

        //Slapper Automapper needs a parameterless constructor
        public UserRole() { }
    }
}
