using System.Diagnostics;
using Application.Interfaces.Data;
using Application.Models;
using Infrastructure.Parser.DataProvider;
using Infrastructure.Parser.Loader;
using Microsoft.Playwright;

namespace Infrastructure.Data
{
    public class UrlContentExtractor : IUrlContentExtractor
    {
        private readonly ILoader _loader;
        private readonly IPageDataProvider _dataProvider;

        public UrlContentExtractor( ILoader loader, IPageDataProvider dataProvider )
        {
            _loader = loader;
            _dataProvider = dataProvider;
        }

        public async Task<Page?> ExtractAsync( string url, string languageShort )
        {
            IPage? page = await _loader.OpenPageAsync( url, languageShort );
            IReadOnlyList<SearchItem>? searchItems = await _dataProvider.GetDataAsync( page );

            Page? result = null;
            try
            {
                result = new Page( url, languageShort, searchItems );
            }
            catch ( ArgumentNullException )
            {
                // log
                Debug.WriteLine( $"Url doesn't have content: {url}" );
            }
            await page.CloseAsync();

            return result;
        }
    }
}
