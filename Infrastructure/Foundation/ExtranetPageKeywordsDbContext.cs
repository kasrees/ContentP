using Microsoft.EntityFrameworkCore;
using Infrastructure.Configurations;

namespace Infrastructure.Foundation
{
    public class ExtranetPageKeywordsDbContext : DbContext
    {
        public ExtranetPageKeywordsDbContext(DbContextOptions<ExtranetPageKeywordsDbContext> options)
                : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PageConfiguration());
            builder.ApplyConfiguration(new LanguageConfiguration());
            builder.ApplyConfiguration(new PageTranslationConfiguration());
            builder.ApplyConfiguration(new KeywordConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
