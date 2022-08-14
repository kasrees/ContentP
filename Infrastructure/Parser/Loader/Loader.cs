using Infrastructure.Parser.Contexts;
using Infrastructure.Parser.Loader.Pages;
using Microsoft.Playwright;

namespace Infrastructure.Parser.Loader
{
    public class Loader : ILoader
    {
        private readonly IPageContext _context;
        private readonly BrowserNewPageOptions _pageOptions;

        public Loader( IPageContext context )
        {
            _context = context;
            _pageOptions = new BrowserNewPageOptions();
        }

        public async Task<IPage> OpenPageAsync( string url, string languageShort )
        {
            AddExtraHeaders( languageShort );
            LoadPageState( languageShort );

            IPage? page = await _context.Browser.NewPageAsync( _pageOptions );
            await new ContentPage( page, url ).LoadContentPage();

            await StorePageStateAsync( page, languageShort );

            return page;
        }

        private void AddExtraHeaders( string languageShort )
        {
            _pageOptions.ExtraHTTPHeaders = new Dictionary<string, string>
            {
                { "Accept-Language", languageShort }
            };
        }

        private void LoadPageState(string languageShort)
        {
            if ( File.Exists( $"States/{languageShort}-state.json" ) )
            {
                _pageOptions.StorageStatePath = $"States/{languageShort}-state.json";
            }
        }

        private static async Task StorePageStateAsync( IPage page, string languageShort )
        {
            await page.Context.StorageStateAsync( new BrowserContextStorageStateOptions
            {
                Path = $"States/{languageShort}-state.json"
            } );
        }
    }
}
