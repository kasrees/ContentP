using Application.Models;
using Application.OptionModels;
using Microsoft.Playwright;

namespace Infrastructure.Parser.ContentExtractor
{
    public interface IPageContentExtractor
    {
        Task<IReadOnlyList<SearchItem>> ExtractAsync( IPage page, SelectorConfig selector );
    }
}
