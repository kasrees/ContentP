namespace KeywordsDashboard.Dtos
{
    public class UpdatePageDto
    {
        public List<PageAttributesDto> PageAttributes { get; set; }
    }
    public class PageAttributesDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public Dictionary<string, int> Keywords { get; set; }
    }
}
