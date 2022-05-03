namespace HotChocolatePOC.Domain.MutationTypes.Inputs
{
    public record AddUserInput(
        string UserName,
        Guid? UserType_Id
    );
}
