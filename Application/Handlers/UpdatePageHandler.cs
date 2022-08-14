using Application.Commands;
using Domain.RepositoryInterfaces;
using Domain.Entities;
using Application.Interfaces.Handlers;

namespace Application.Handlers
{
    public class UpdatePageHandler : ICommandHandler<UpdatePageCommand, int?>
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private const int MaxTitleLength = 100;
        private const int MaxDescriptionLength = 200;
        private const int MaxKeywordsCount = 30;

        public UpdatePageHandler(IPageRepository pageRepository, ILanguageRepository languageRepository, IUnitOfWork unitOfWork)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<int?> Handle(UpdatePageCommand command)
        {
            Page? page = await _pageRepository.GetByIdAsync(command.Id);
            if (page == null)
            {
                return null;
            }

            foreach (PageAttributes pageAttributes in command.PageAttributes)
            {
                Language? language = await _languageRepository.GetByIdAsync(pageAttributes.LanguageId);
                if (language == null)
                {
                    throw new ArgumentException("Invalid parameters");
                }

                List<Keyword> keywords = new List<Keyword>();

                var pageTranslation = page.PageTranslations.Where(pT => pT.LanguageId == language.Id).FirstOrDefault();

                if (pageTranslation != null)
                {
                    page.PageTranslations.Remove(pageTranslation);
                }

                if (!ArePageAttributesCorrect(pageAttributes))
                {
                    throw new ArgumentException("Invalid parameters");
                }

                // create
                var newPageTranslation = new PageTranslation(pageAttributes.Title, pageAttributes.Description, page.Id, language.Id, keywords);
                foreach (KeyValuePair<string, int> phrase in pageAttributes.Keywords)
                {
                    keywords.Add(new Keyword(newPageTranslation.Id, phrase.Key, phrase.Value));
                }

                newPageTranslation.Keywords.AddRange(keywords);
                page.PageTranslations.Add(newPageTranslation);
            }
            await _unitOfWork.CommitAsync();
            return page.Id;
        }

        private bool ArePageAttributesCorrect(PageAttributes pageAttributes)
        {
            return
                pageAttributes.Title.Length > 0 && pageAttributes.Title.Length <= MaxTitleLength &&
                pageAttributes.Description.Length <= MaxDescriptionLength &&
                pageAttributes.Keywords.Count > 0 && pageAttributes.Keywords.Count <= MaxKeywordsCount;
        }
    }
}
