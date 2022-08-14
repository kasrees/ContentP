using Application.Models;

namespace Application.Interfaces.Data
{
    public interface IUrlContentExtractor
    {
        Task<Page?> ExtractAsync( string url, string languageShort );
    }
}