using Application.Commands;
using Application.Services;
using Domain.RepositoryInterfaces;
using Domain.Entities;
using Application.Interfaces.Handlers;

namespace Application.Handlers
{
    public class AddPageHandler : ICommandHandler<AddPageCommand, int?>
    {
        private readonly IPageRepository _pageRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddPageHandler(IPageRepository pageRepository, IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int?> Handle(AddPageCommand command)
        {
            string? relativeUri = LinkHelper.getRelativeUriFromAbsolute(command.Link);
            if (string.IsNullOrEmpty(relativeUri))
            {
                throw new ArgumentException("Invalid parameters");
            }
            Page? page = await _pageRepository.GetByLinkAsync(relativeUri);
            if (page != null)
            {
                return null;
            }
            Page newPage = new Page(relativeUri);
            _pageRepository.Add(newPage);
            await _unitOfWork.CommitAsync();
            return newPage.Id;
        }
    }
}
