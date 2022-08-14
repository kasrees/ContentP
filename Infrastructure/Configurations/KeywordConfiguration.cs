using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.ToTable(nameof(Keyword)).HasKey(t => t.Id);

            builder
                .HasOne<PageTranslation>()
                .WithMany(pT => pT.Keywords)
                .HasForeignKey(k => k.PageTranslationId);
        }
    }
}
