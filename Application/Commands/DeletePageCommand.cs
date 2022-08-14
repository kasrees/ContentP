using Application.Interfaces.Commands;

namespace Application.Commands
{
    public class DeletePageCommand : ICommand
    {
        public int Id { get; private set; }

        public DeletePageCommand(int id)
        {
            this.Id = id;
        }
    }
}
