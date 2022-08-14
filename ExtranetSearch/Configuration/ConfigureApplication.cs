using Application.Interfaces.Data;
using Application.Interfaces.Services;
using Application.OptionModels;
using Application.Services;
using Infrastructure.Data;
using Infrastructure.Parser.DataProvider;
using Infrastructure.Parser.Loader;
using Nest;

namespace FullTextExtranetSearch.Configuration
{
    public static class ConfigureApplication
    {
        public static void ConfigureContentCollector( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddScoped<IUrlContentExtractor>( s =>
            {
                var loader = s.GetRequiredService<ILoader>();
                var dataProvider = s.GetRequiredService<IPageDataProvider>();

                return new UrlContentExtractor( loader, dataProvider );
            } );
        }

        public static void ConfigureIndexer( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddScoped<IIndexCreationService, IndexCreationService>();

            services.AddScoped<IIndexDataService>( s =>
            {
                var client = s.GetRequiredService<IElasticClient>();
                var extractor = s.GetRequiredService<IUrlContentExtractor>();
                var indexName = configuration.GetSection( "DefaultIndex" ).Get<string>();
                var urls = configuration.GetSection( "urls" ).Get<List<string>>();

                return new IndexDataService( client, extractor, urls );
            } );

            services.AddScoped<IIndexSynchronizationService>( s =>
            {
                var indexer = s.GetRequiredService<IIndexDataService>();
                var indices = configuration.GetSection( "Indices" ).Get<IReadOnlyList<IndexConfig>>();
                var indexCreation = s.GetRequiredService<IIndexCreationService>();

                return new IndexSynchronizationService( indexer, indexCreation, indices );
            } );
        }

        public static void ConfigureSearcher( this IServiceCollection services )
        {
            services.AddScoped<ISearchService, SearchService>();
        }
    }
}
