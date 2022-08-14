namespace Domain.Entities
{
    public class Page
    {
        public int Id { get; private set; }
        public string Link { get; private set; }
        public DateTime CreatedAt { get; private set; }
        //Navigation Properties
        public List<PageTranslation> PageTranslations { get; private set; }

        public Page(string link)
        {
            Link = link;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
