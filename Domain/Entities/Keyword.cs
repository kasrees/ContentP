namespace Domain.Entities
{
    public class Keyword
    {
        public int Id { get; private set; }
        public string Phrase { get; private set; }
        public int Order { get; private set; }
        public int PageTranslationId { get; private set; }

        public Keyword(int pageTranslationId, string phrase, int order)
        {
            PageTranslationId = pageTranslationId;
            Phrase = phrase;
            Order = order;
        }
    }
}
