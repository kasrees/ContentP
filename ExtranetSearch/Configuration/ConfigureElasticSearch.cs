using Nest;

namespace FullTextExtranetSearch.Configuration
{
    public static class ConfigureElasticSearch
    {
        public static void ConfigureElastic( this IServiceCollection services, IConfiguration configuration )
        {
            var settings = new ConnectionSettings( new Uri( configuration.GetValue<string>( "ConnectionString" ) ) )
                .DefaultIndex( configuration.GetValue<string>( "DefaultIndex" ) )
                .DefaultMappingFor<Application.Models.Page>( i => i
                    .IndexName( configuration.GetValue<string>( "DefaultIndex" ) )
                );

            var client = new ElasticClient( settings );

            services.AddSingleton<IElasticClient>( s => client );
        }
    }
}
