using System.Diagnostics;
using Microsoft.Playwright;

namespace Infrastructure.Parser.Contexts
{
    public class PageContext : IPageContext
    {
        public IPlaywright Context { get; private set; }
        public IBrowser Browser { get; private set; }

        public PageContext( BrowserTypeLaunchOptions launchOptions )
        {
            RunAsync( launchOptions ).Wait();
        }
        public async Task RunAsync( BrowserTypeLaunchOptions launchOptions )
        {
            Context = await Playwright.CreateAsync();
            Browser = await Context.Chromium.LaunchAsync( launchOptions );
        }

        public void Dispose()
        {
            // Нужно для гарантированного закрытия браузера и освобождения ресурсов
            Browser.DisposeAsync()
                .GetAwaiter()
                .GetResult();
            Context?.Dispose();
            Debug.WriteLine( $"{nameof( PageContext )} was dispose" );
        }
    }
}
