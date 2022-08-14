using Application.Interfaces.Data;
using Application.Interfaces.Services;
using Application.OptionModels;
using Nest;

namespace Application.Services
{
    public class IndexDataService : IIndexDataService
    {
        private readonly IElasticClient _client;
        private readonly IUrlContentExtractor _extractor;
        private readonly List<string> _urls;

        public IndexDataService( IElasticClient client, IUrlContentExtractor extractor, List<string> urls )
        {
            _client = client;
            _extractor = extractor;
            _urls = urls;
        }

        public async Task IndexDataAsync( IndexConfig indexConfig )
        {
            foreach ( var url in _urls )
            {
                Models.Page? page = await _extractor.ExtractAsync( url, indexConfig.Language.LanguageShortName );
                if ( page is null )
                {
                    continue;
                }
                await _client.IndexAsync( page, i => i.Index( indexConfig.IndexName ) );
            }
        }
    }
}
