using Application.Queries.Dtos;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IPageService
    {
        public Task<PageDetailWithAllTranslationsDto?> GetPageDetailWithAllTranslationsAsync(int pageId);
        public Task<PageIndexDto?> GetPageIndexAsync(GetPageIndexRequestDto getPageIndexRequestDto);
        public List<int> GetLanguagesOfTitleDuplicates(GetPageTitleDuplicatesRequestDto getPageByTitleRequestDto);
    }
}
