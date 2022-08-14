using Application.Interfaces.Commands;

namespace Application.Interfaces.Handlers
{
    public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
    {
        Task<TResult> Handle(TCommand command);
    }
}
