using KeywordsDashboard.Dtos;
using Application.Commands;

namespace KeywordsDashboard.Mappers
{
    public static class UpdatePageMapper
    {
        public static UpdatePageCommand Map(int id, UpdatePageDto updatePageDto)
        {
            List<PageAttributes> pageAttributes = new List<PageAttributes>();
            foreach (var pageData in updatePageDto.PageAttributes) {
                PageAttributes attributes = new PageAttributes
                {
                    Title = pageData.Title,
                    Description = pageData.Description,
                    LanguageId = pageData.LanguageId,
                    Keywords = pageData.Keywords
                };
                pageAttributes.Add(attributes);
            }
            return new UpdatePageCommand(id, pageAttributes);
        }
    }
}
