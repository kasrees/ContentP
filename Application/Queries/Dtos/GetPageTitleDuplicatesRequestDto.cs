namespace Application.Queries.Dtos
{
    public class GetPageTitleDuplicatesRequestDto
    {
        public int PageId { get; set; }
        public List<string> Titles { get; set; }
    }
}

