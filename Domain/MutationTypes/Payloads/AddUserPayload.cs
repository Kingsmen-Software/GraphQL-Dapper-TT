using HotChocolatePOC.Domain.Entities;

namespace HotChocolatePOC.Domain.MutationTypes.Payloads
{
    public class AddUserPayload
    {
        public AddUserPayload(User user)
        {
            User = user;
        }

        public User User { get; }
    }
}
