using Microsoft.Playwright;

namespace Infrastructure.Parser.Contexts
{
    public interface IPageContext : IDisposable
    {
        IBrowser Browser { get; }
        IPlaywright Context { get; }
        Task RunAsync( BrowserTypeLaunchOptions launchOptions );
    }
}