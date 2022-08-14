using Application.Enums;

namespace Application.Queries.Dtos
{
    public class GetPageIndexRequestDto
    {
        public int LanguageId { get; set; }
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public PageSortCriteria SortField { get; set; }
        public SortDirectionCriteria SortDirection { get; set; }
    }
}
