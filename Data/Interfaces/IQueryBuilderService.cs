using HotChocolatePOC.Data.Classes;

using HotChocolate.Language;

namespace HotChocolatePOC.Data.Interfaces
{
    public interface IQueryBuilderService
    {
        Task<QueryTemplate> BuildQueryTemplateAsync(SelectionSetNode selectionSet);
        Task<QueryTemplate> BuildQueryTemplateAsync(SelectionSetNode selectionSet, Guid? id);
    }
}
