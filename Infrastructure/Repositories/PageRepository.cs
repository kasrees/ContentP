using Domain.RepositoryInterfaces;
using Domain.Entities;
using Infrastructure.Foundation;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class PageRepository : Repository<Page>, IPageRepository
    {
        public PageRepository(ExtranetPageKeywordsDbContext dbContext ) : base( dbContext )
        { }

        public async Task<Page?> GetByIdAsync(int id)
        {
            return await Entities.Include(p => p.PageTranslations).ThenInclude(pT => pT.Keywords).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Page?> GetByLinkAsync(string link)
        {
            return await Entities.FirstOrDefaultAsync(p => p.Link == link);
        }

        public async Task<Page?> GetByIdAndLanguageIdAsync(int id, int languageId)
        {
            return await Entities
                .Where(p => p.Id == id)
                .Include(p => p.PageTranslations
                .Where(pT => pT.LanguageId == languageId))
                .Include(p => p.PageTranslations)
                .ThenInclude(pT => pT.Keywords)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Page>?> GetAllAsync()
        {
            return await Entities
                .Include(p => p.PageTranslations)
                .ToListAsync();
        }
        public List<int> GetLanguagesOfDuplicatedTitles(int pageId, List<string> titles)
        {
            List<int> pageLanguageIds = new List<int>();   
            foreach (string title in titles) 
            {
               Page? page = Entities
                   .Include(p => p.PageTranslations)
                   .Where(p => p.PageTranslations.Any(pT => pT.Title == title && pT.PageId != pageId)).FirstOrDefault();
                if (page != null)
                {
                    IEnumerable<int> languageIds = page.PageTranslations.Where(pT => pT.Title == title).Select(pT => pT.LanguageId);
                    pageLanguageIds.AddRange(languageIds.ToList());
                }
            }
            return pageLanguageIds;
        }
    }
}
