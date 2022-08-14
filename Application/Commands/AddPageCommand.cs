using Application.Interfaces.Commands;

namespace Application.Commands
{
    public class AddPageCommand : ICommand
    {
        public string Link { get; private set; }

        public AddPageCommand(string link)
        {
            this.Link = link;
        }
    }
}
