using System.Diagnostics;
using Infrastructure.Extensions;
using Infrastructure.Parser.Config;
using Microsoft.Playwright;

namespace Infrastructure.Parser.Loader.Pages
{
    internal class ContentPage
    {
        private const int _retryCount = 2;
        private readonly IPage _page;
        private readonly string _contentUrl;
        private readonly string _preloadUrl;
        private readonly LocatorWaitForOptions _waitOptions;
        private readonly PageGotoOptions _gotoOptions;

        public ContentPage( IPage page, string contentUrl )
        {
            _page = page;
            _contentUrl = contentUrl;
            _preloadUrl = "https://www.travelline.ru/secure/SwitchUserContext.ashx?ReturnUrl=%2fsecure%2fwebpms%2fextranet%2f&App=Management&Context=8";
            _waitOptions = new LocatorWaitForOptions
            {
                Timeout = 10000
            };
            _gotoOptions = new PageGotoOptions
            {
                Timeout = 15000,
                WaitUntil = WaitUntilState.Load
            };
        }

        public async Task LoadContentPage()
        {
            await LoadPageAsync( _preloadUrl, _retryCount );
            await LoginAsync();
            await VerificateAsync();
            await LoadPageAsync( _contentUrl, _retryCount );
            await LoginAsync();
            await VerificateAsync();

            await WaitForLoadContentPageAsync();
        }

        private async Task LoadPageAsync( string url, int countRetry )
        {
            for( int i = 0; i < countRetry; )
            {
                try
                {
                    await _page.GotoAsync( url, _gotoOptions );
                    break;
                }
                catch ( TimeoutException )
                {
                    //log
                    i++;
                    Debug.WriteLine( $"Attempt: {i}. Can't load page with url: {url}" );
                }
            }
        }

        private async Task LoginAsync()
        {
            if ( _page.Url.GetParameterlessUrl() == PlaywrightConfig.LoginUrl )
            {
                await _page.FillAsync( "input[name='username']", PlaywrightConfig.Username );
                await _page.FillAsync( "input[name='password']", PlaywrightConfig.Password );
                await _page.ClickAsync( "tl-button[ng-click='login()']" );
                await _page.WaitForLoadStateAsync( LoadState.DOMContentLoaded );
            }
        }

        private async Task VerificateAsync()
        {
            if ( _page.Url.GetParameterlessUrl() == PlaywrightConfig.ConfirmUrl )
            {
                //need another implementaition of code verification
                Console.WriteLine( "Enter code:" );
                string verificationCode = Console.ReadLine();

                await _page.FillAsync( "input[name='secondFactorCode']", verificationCode );
                await _page.ClickAsync( "tl-button[tl-click='checkSecurityCode()']" );
                await _page.WaitForLoadStateAsync( LoadState.DOMContentLoaded );
            }
        }

        private async Task WaitForLoadContentPageAsync()
        {
            try
            {
                await _page.Locator( "h1" ).First.WaitForAsync( _waitOptions );
                await _page.WaitForLoadStateAsync( LoadState.NetworkIdle );
            }
            catch ( TimeoutException )
            {
                Debug.WriteLine( $"Can't find h1 tag on page: {_page.Url}" );
            }
            await _page.ScreenshotAsync( new PageScreenshotOptions { Path = $"Pictures/{Guid.NewGuid()}.png" } );
        }
    }
}
