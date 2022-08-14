using Microsoft.Playwright;

namespace Infrastructure.Parser.Loader
{
    public interface ILoader
    {
        Task<IPage> OpenPageAsync( string url , string languageShort );
    }
}
