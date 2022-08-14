namespace Domain.Entities
{
    public class PageDetailWithAllTranslationsDto
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public List<PageTranslationDto> PageTranslations { get; set; }
    }

    public class PageTranslationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public List<KeywordDto> Keywords { get; set; }
    }

    public class KeywordDto
    {
        public int Id { get; set; }
        public string Phrase { get; set; }
        public int Order { get; set; }
    }
}
