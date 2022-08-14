using Application.Models;
using Application.OptionModels;
using Microsoft.Playwright;

namespace Infrastructure.Parser.ContentExtractor
{
    public class InnerTextContentExtractor : IPageContentExtractor
    {
        public async Task<IReadOnlyList<SearchItem>> ExtractAsync( IPage page, SelectorConfig selector )
        {
            IReadOnlyList<IElementHandle>? elements = await page.QuerySelectorAllAsync( selector.Tag );
            List<SearchItem> result = new();
            foreach ( var element in elements )
            {
                string? content = await element.InnerTextAsync();
                if ( content != null )
                {
                    SearchItem searchItem;
                    try
                    {
                        searchItem = new SearchItem( page.Url, selector.Tag, content, selector.Weight );
                    }
                    catch ( ArgumentNullException )
                    {
                        continue;
                    }
                    result.Add( searchItem );
                }
            }
            return result;
        }
    }
}
