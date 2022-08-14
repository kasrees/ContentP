using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PageTranslationConfiguration : IEntityTypeConfiguration<PageTranslation>
    {
        public void Configure(EntityTypeBuilder<PageTranslation> builder)
        {
            builder.ToTable(nameof(PageTranslation)).HasKey(t => t.Id);

            builder
                .HasOne<Page>()
                .WithMany(p => p.PageTranslations)
                .HasForeignKey(pT => pT.PageId);

            builder
                .HasOne<Language>()
                .WithMany()
                .HasForeignKey(pT => pT.LanguageId);

            builder
                .HasMany(pT => pT.Keywords)
                .WithOne();
        }
    }
}
