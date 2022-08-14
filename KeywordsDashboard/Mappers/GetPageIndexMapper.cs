using Application.Enums;
using Application.Queries.Dtos;
using KeywordsDashboard.Dtos;

namespace KeywordsDashboard.Mappers
{
    public static class GetPageIndexMapper
    {
        public static GetPageIndexRequestDto Map(GetPageIndexRequest getPageIndexRequest)
        {
            PageSortCriteria sortField;
            SortDirectionCriteria sortDirection;
            if (!SortDirectionCriteria.TryParse(capitalizeFirstLetter(getPageIndexRequest.SortField), out sortField) ||
                !SortDirectionCriteria.TryParse(capitalizeFirstLetter(getPageIndexRequest.SortDirection), out sortDirection))
            {
                throw new ArgumentException("Invalid parameter");
            }
            return new GetPageIndexRequestDto() {
                LanguageId = getPageIndexRequest.LanguageId,
                PageNumber = getPageIndexRequest.PageNumber,
                Limit = getPageIndexRequest.Limit,
                SortField = sortField,
                SortDirection = sortDirection
            };
        }

        public static string capitalizeFirstLetter(string text)
        {
            if (text.Length == 0)
            {
                return "";
            }
            if (text.Length == 1)
            {
                return char.ToUpper(text[0]).ToString();
            }
            return char.ToUpper(text[0]) + text.Substring(1);
        }
    }
}
