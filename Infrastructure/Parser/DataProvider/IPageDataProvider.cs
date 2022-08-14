using Application.Models;
using Microsoft.Playwright;

namespace Infrastructure.Parser.DataProvider
{
    public interface IPageDataProvider
    {
        Task<IReadOnlyList<SearchItem>> GetDataAsync( IPage page );
    }
}