using Application.Models;
using Application.OptionModels;
using Infrastructure.Parser.ContentExtractor;
using Microsoft.Playwright;

namespace Infrastructure.Parser.DataProvider
{
    public class PageDataProvider : IPageDataProvider
    {
        private readonly IPageContentExtractor _contentExtractor;
        private readonly IEnumerable<SelectorConfig> _selectors;

        public PageDataProvider( IPageContentExtractor contentExtractor, IEnumerable<SelectorConfig> selectors )
        {
            _contentExtractor = contentExtractor;
            _selectors = selectors;
        }

        public async Task<IReadOnlyList<SearchItem>> GetDataAsync( IPage page )
        {
            List<SearchItem> pageSelectorsContentResult = new();
            foreach ( var selector in _selectors )
            {
                IReadOnlyList<SearchItem> items = await _contentExtractor.ExtractAsync( page, selector );
                pageSelectorsContentResult.AddRange( items );
            }
            return pageSelectorsContentResult;
        }


    }
}
