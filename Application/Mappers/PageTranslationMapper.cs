using Domain.Entities;

namespace Application.Mappers
{
    public class PageTranslationMapper
    {
        public static PageTranslationDto Map(int id, string title, string description, int languageId, List<KeywordDto> keywords)
        {
            return new PageTranslationDto
            {
                Id = id,
                Title = title,
                Description = description,
                LanguageId = languageId,
                Keywords = keywords
            };
        }
    }
}
