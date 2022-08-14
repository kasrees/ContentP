namespace Domain.Entities
{
    public class PageTranslation
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public int PageId { get; private set; }
        public int LanguageId { get; private set; }
        //Navigation Properties
        public List<Keyword> Keywords { get; private set; }

        public PageTranslation(string title, string description, int pageId, int languageId, List<Keyword> keywords)
        {
            Title = title;
            Description = description;
            PageId = pageId;
            LanguageId = languageId;
            Keywords = keywords;
        }

        protected PageTranslation()
        {
        }
    }
}
