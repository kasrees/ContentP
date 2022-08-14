using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface IPageRepository : IRepository<Page>
    {
        Task<Page?> GetByIdAsync(int Id);
        Task<Page?> GetByLinkAsync(string link);
        Task<Page?> GetByIdAndLanguageIdAsync(int Id, int languageId);
        Task<List<Page>?> GetAllAsync();
        List<int> GetLanguagesOfDuplicatedTitles(int pageId, List<string> titles);
    }
}
