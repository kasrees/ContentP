using KeywordsDashboard.Dtos;
using Application.Commands;

namespace KeywordsDashboard.Mappers
{
    public static class AddPageMapper
    {
        public static AddPageCommand Map(AddPageDto addPageDto)
        {
            return new AddPageCommand(addPageDto.Link);
        }
    }
}
