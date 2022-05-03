using HotChocolatePOC.Domain.MutationTypes.Inputs;
using HotChocolatePOC.Domain.MutationTypes.Payloads;
using HotChocolatePOC.Domain.Entities;
using HotChocolatePOC.Data.Classes;
using HotChocolatePOC.Data.Interfaces;
using HotChocolatePOC.Domain.MutationTypes.InputExtensions;

namespace HotChocolatePOC.GraphQL.Mutation
{
    public class Mutation
    {
        public async Task<AddUserPayload> AddUserAsync(
            AddUserInput input,
            [Service] IMutationBuilderService builder,
            [Service] IMutationExecuteService executor
            )
        {
            try
            {
                MutationTemplate mutationTemplate = builder.BuildMutationTemplate(new User(input));

                User result = await executor.ExecuteInsertAsync<User>(mutationTemplate);

                return new AddUserPayload(result);
            }
            catch (Exception ex)
            {
                //handle exception however you need to
                return null;
            }
        }

        public async Task<IEnumerable<AddUserPayload>> AddUsersAsync(
            IEnumerable<AddUserInput> inputs,
            [Service] IMutationBuilderService builder,
            [Service] IMutationExecuteService executor
            )
        {
            try
            {
                MutationTemplate mutationTemplate = builder.BuildMutationTemplate(inputs.ToClass<User, AddUserInput>());

                IEnumerable<User> results = await executor.ExecuteInsertMultipleAsync<User>(mutationTemplate);

                return results.ToClass<AddUserPayload, User>();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
