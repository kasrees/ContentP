namespace Application.Dto
{
    public class ResultDto
    {
        public IReadOnlyList<SearchResultDto> SearchResults { get; set; }
        public IReadOnlyList<SuggestResultDto> SuggestResults { get; set; }
    }
}
