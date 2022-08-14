using Domain.Entities;

namespace Domain.RepositoryInterfaces
{
    public interface ILanguageRepository : IRepository<Language>
    {
        Task<Language?> GetByIdAsync(int id);
        Task<Language?> GetByCodeAsync(string code);
        Task<List<Language>?> GetAllAsync();
    }
}
