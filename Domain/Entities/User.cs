using System.ComponentModel.DataAnnotations.Schema;

using HotChocolatePOC.Domain.Classes;
using HotChocolatePOC.Domain.MutationTypes.Inputs;

namespace HotChocolatePOC.Domain.Entities
{
    public class User : Entity
    { 
        public string? UserName { get; set; }

        public Guid? UserType_Id { get; set; }
        [ForeignKey("UserType_Id")]
        public UserType? UserType { get; set; }

        public ICollection<UserRole>? UserRoles { get; set; }

        public User(AddUserInput input)
        {
            UserName = input.UserName;
            UserType_Id = input.UserType_Id ?? null;
        }

        //Slapper Automapper needs a parameterless constructor
        public User() { }
    }
}
