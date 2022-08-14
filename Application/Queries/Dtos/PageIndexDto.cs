namespace Application.Queries.Dtos
{
    public class PageDataDto
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Dictionary<string, object>> Languages { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CurrentLanguageId { get; set; }
    }

    public class PageIndexDto
    {
        public List<PageDataDto> Pages { get; set; }
        public int PagesCount { get; set; }
    }
}