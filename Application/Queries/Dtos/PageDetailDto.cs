namespace Application.Queries.Dtos
{
    public class PageDetailDto
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Dictionary<int, string> Keywords { get; set; }
    }
}
