using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PageConfiguration : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.ToTable(nameof(Page)).HasKey(t => t.Id);

            builder
                .HasMany(p => p.PageTranslations)
                .WithOne();
        }
    }
}
