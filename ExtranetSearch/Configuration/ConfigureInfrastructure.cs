using Application.OptionModels;
using Infrastructure.Parser.ContentExtractor;
using Infrastructure.Parser.Contexts;
using Infrastructure.Parser.DataProvider;
using Infrastructure.Parser.Loader;
using Microsoft.Playwright;

namespace FullTextExtranetSearch.Configuration
{
    public static class ConfigureInfrastructure
    {
        public static void ConfigureParser( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddTransient<IPageContext>( s =>
            {
                BrowserTypeLaunchOptions launchOptions = new()
                {
                    Channel = "chrome",
                    Headless = true,
                };

                return new PageContext( launchOptions );
            } );

            services.AddScoped<ILoader, Loader>();
            services.AddScoped<IPageContentExtractor, InnerTextContentExtractor>();

            services.AddScoped<IPageDataProvider>( s =>
            {
                var extractor = s.GetRequiredService<IPageContentExtractor>();
                var selectors = configuration.GetSection( "selectors" ).Get<List<SelectorConfig>>();

                return new PageDataProvider( extractor, selectors );
            } );
        }
    }
}
