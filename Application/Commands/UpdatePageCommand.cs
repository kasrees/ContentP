using Application.Interfaces.Commands;

namespace Application.Commands
{
    public class UpdatePageCommand : ICommand
    {
        public int Id { get; private set; }
        public List<PageAttributes> PageAttributes { get; private set; }
        public UpdatePageCommand(int id, List<PageAttributes> pageAttributes)
        {
            this.Id = id;
            this.PageAttributes = pageAttributes;
        }
    }

    public class PageAttributes
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public Dictionary<string, int> Keywords { get; set; }
    }
}
