using Application.Queries.Dtos;
using Domain.RepositoryInterfaces;
using Domain.Entities;
using Application.Interfaces.Services;
using Application.Enums;
using Application.Mappers;

namespace Application.Services
{
    public class PageService : IPageService
    {
        private readonly IPageRepository _pageRepository;
        private readonly ILanguageRepository _languageRepository;

        public PageService(IPageRepository pageRepository, ILanguageRepository languageRepository)
        {
            _pageRepository = pageRepository;
            _languageRepository = languageRepository;
        }

        public async Task<PageDetailWithAllTranslationsDto?> GetPageDetailWithAllTranslationsAsync(int pageId)
        {
            Page? page = await _pageRepository.GetByIdAsync(pageId);
            if (page is null)
            {
                return null;
            }

            List<PageTranslationDto> pageTranslationDtos = new List<PageTranslationDto>();
            foreach (PageTranslation pageTranslation in page.PageTranslations)
            {
                List<KeywordDto> keywordDtos = new List<KeywordDto>();
                foreach (Keyword keyword in pageTranslation.Keywords)
                {
                    KeywordDto keywordDto = KeywordMapper.Map(keyword.Id, keyword.Phrase, keyword.Order);
                    keywordDtos.Add(keywordDto);
                }
                keywordDtos.OrderBy(k => k.Order);
                PageTranslationDto ptDto = PageTranslationMapper.Map(pageTranslation.Id, pageTranslation.Title, pageTranslation.Description, pageTranslation.LanguageId, keywordDtos);
                pageTranslationDtos.Add(ptDto);
            }
            PageDetailWithAllTranslationsDto pageWithTranslations = PageDetailWithAllTranslationsMapper.Map(page.Id, page.Link, pageTranslationDtos);
            return pageWithTranslations;
        }

        public async Task<PageIndexDto?> GetPageIndexAsync(GetPageIndexRequestDto getPageIndexRequestDto)
        {
            if (getPageIndexRequestDto.PageNumber == 0 || getPageIndexRequestDto.Limit == 0)
            {
                throw new ArgumentException("Invalid parameter");
            }

            List<Page>? pages = await _pageRepository.GetAllAsync();
            if (pages is null)
            {
                return null;
            }

            Language? language = await _languageRepository.GetByIdAsync(getPageIndexRequestDto.LanguageId);
            if (language is null)
            {
                throw new ArgumentException("Invalid parameter");
            }

            List<PageDataDto> pageIndex = await this.MapPagesToListOfPageDataDtoAsync(pages, getPageIndexRequestDto.LanguageId);
            int skipNumber = (getPageIndexRequestDto.PageNumber - 1) * getPageIndexRequestDto.Limit;
            List<PageDataDto> pagesList = SelectPages(pageIndex, getPageIndexRequestDto.Limit, skipNumber, getPageIndexRequestDto.SortField, getPageIndexRequestDto.SortDirection);
            return new PageIndexDto { Pages = pagesList, PagesCount = pageIndex.Count };
        }

        public List<int> GetLanguagesOfTitleDuplicates(GetPageTitleDuplicatesRequestDto getPageByTitleRequestDto)
        {
            var page = _pageRepository.GetLanguagesOfDuplicatedTitles(getPageByTitleRequestDto.PageId, getPageByTitleRequestDto.Titles);
            return page;
        }

        private List<PageDataDto> SelectPages
        (
            List<PageDataDto> pageData,
            int pagesNumberToTake,
            int pagesNumberToSkip,
            PageSortCriteria sortField,
            SortDirectionCriteria sortDirection
        )
        {
            IEnumerable<PageDataDto> sortedData = pageData.Where(p => p.Title != "");
            string sortFieldString = sortField.ToString();
            if (sortField.Equals(PageSortCriteria.Title) && sortDirection.Equals(SortDirectionCriteria.Asc))
            {
                sortedData = sortedData.OrderBy(p => p.Title);
            }
            else if (sortField.Equals(PageSortCriteria.Title) && sortDirection.Equals(SortDirectionCriteria.Desc))
            {
                sortedData = sortedData.OrderByDescending(p => p.Title);
            }
            else
            {
                sortedData = sortedData.OrderByDescending(p => p.CreatedAt);
            }

            return pageData
                .Where(p => p.Title == "")
                .OrderByDescending(p => p.CreatedAt)
                .Union(sortedData)
                .Skip(pagesNumberToSkip)
                .Take(pagesNumberToTake)
                .ToList();
        }

        private PageDataDto GeneratePageIndexDtoWithoutTranslation(Page page)
        {
            return new PageDataDto
            {
                Id = page.Id,
                Link = page.Link,
                Title = "",
                Description = "",
                Languages = new List<Dictionary<string, object>>(),
                CurrentLanguageId = null,
                CreatedAt = page.CreatedAt
            };
        }

        private async Task<List<Dictionary<string, object>>> GetAvailablePageLanguagesAsync(List<PageTranslation> pageTranslations)
        {
            List<Language>? allLanguages = await _languageRepository.GetAllAsync();
            Dictionary<int, string>? languagesMap = allLanguages?.ToDictionary(x => x.Id, x => x.Code);
            List<int> allPageLanguageIds = pageTranslations.Select(pT => pT.LanguageId).ToList();
            List<Dictionary<string, object>> languages = new List<Dictionary<string, object>>();
            allPageLanguageIds.ForEach(langId => languages.Add(new Dictionary<string, object>()
                {
                    { "id", langId },
                    { "code", languagesMap[langId] }
                }));
            return languages;
        }

        private async Task<PageDataDto?> GeneratePageIndexDtoWithTranslationAsync(Page page, List<PageTranslation> pageTranslations, int requestedLanguage)
        {
            Language? englishLanguage = await _languageRepository.GetByCodeAsync("en");
            PageTranslation? pageTranslation = pageTranslations.Where(pT => pT.LanguageId == requestedLanguage).FirstOrDefault();
            if (pageTranslation is null)
            {
                if (englishLanguage is null)
                {
                    return null;
                }
                pageTranslation = pageTranslations.Where(pT => pT.LanguageId == englishLanguage.Id).FirstOrDefault();
                if (pageTranslation == null)
                {
                    return null;
                }
            }

            List<Dictionary<string, object>> languages = await GetAvailablePageLanguagesAsync(pageTranslations);

            return new PageDataDto
            {
                Id = page.Id,
                Link = page.Link,
                Title = pageTranslation.Title,
                Description = pageTranslation.Description ?? "",
                Languages = languages,
                CurrentLanguageId = pageTranslation.LanguageId,
                CreatedAt = page.CreatedAt
            };
        }

        private async Task<List<PageDataDto>> MapPagesToListOfPageDataDtoAsync(List<Page> pages, int requestedLanguage)
        {
            List<PageDataDto> pageIndex = new List<PageDataDto>();
            foreach (Page page in pages)
            {
                List<PageTranslation> pageTranslations = page.PageTranslations;
                PageDataDto? pageDataDto = pageTranslations.Count == 0
                    ? this.GeneratePageIndexDtoWithoutTranslation(page)
                    : await this.GeneratePageIndexDtoWithTranslationAsync(page, pageTranslations, requestedLanguage);

                if (pageDataDto != null)
                {
                    pageIndex.Add(pageDataDto);
                }
            }
            return pageIndex;
        }
    }
}
