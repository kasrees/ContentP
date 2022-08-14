using Application.Commands;
using Domain.RepositoryInterfaces;
using Domain.Entities;
using Application.Interfaces.Handlers;

namespace Application.Handlers
{
    public class DeletePageHandler : ICommandHandler<DeletePageCommand, int?>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePageHandler(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int?> Handle(DeletePageCommand command) 
        {
            Page? page = await _pageRepository.GetByIdAsync(command.Id);
            if (page == null)
            {
                return null;
            }
            _pageRepository.Remove(page);
            await _unitOfWork.CommitAsync();
            return page.Id;
        }
    }
}
