using Application.Enums;

namespace KeywordsDashboard.Dtos
{
    public class GetPageIndexRequest
    {
        public int LanguageId { get; set; }
        public int PageNumber { get; set; }
        public int Limit { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
    }
}
