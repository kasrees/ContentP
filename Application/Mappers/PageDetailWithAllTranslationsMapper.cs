using Domain.Entities;

namespace Application.Mappers
{
    public class PageDetailWithAllTranslationsMapper
    {
        public static PageDetailWithAllTranslationsDto Map(int id, string link, List<PageTranslationDto> pageTranslations)
        {
            return new PageDetailWithAllTranslationsDto
            {
                Id = id,
                Link = link,
                PageTranslations = pageTranslations
            };
        }
    }
}
