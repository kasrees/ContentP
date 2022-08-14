using Domain.RepositoryInterfaces;
using Infrastructure.Foundation;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        public LanguageRepository(ExtranetPageKeywordsDbContext dbContext ) : base( dbContext )
        { 
        }

        public async Task<Language?> GetByIdAsync(int id)
        {
            return await Entities.FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Language?> GetByCodeAsync(string code)
        {
            return await Entities.FirstOrDefaultAsync(l => l.Code == code);
        }

        public async Task<List<Language>?> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }
    }
}
