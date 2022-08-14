using Application.Dto;
using Application.Interfaces.Services;
using Nest;

namespace Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly IElasticClient _client;
        private readonly string _suggestionName;

        public SearchService( IElasticClient client )
        {
            _client = client;
            _suggestionName = "suggestions";
        }

        public ResultDto Search( string query, int size )
        {
            ISearchResponse<PageDto>? response = _client.Search<PageDto>( search => search
                .Index(new [] { "extranet-search-russian", "extranet-search-english" } )
                // обозначаем поля для вывода
                .Source( source => source
                    .Includes( i => i
                        .Field( f => f.Title )
                        .Field( f => f.Description )
                        .Field( f => f.Url )
                    )
                )
                .Size( size )
                // В зависимости от количества совпадений увеличивается релевантность запроса
                .Query( q => q
                    .Bool( b => b
                        .Must( m => m
                            .Bool( bo => bo
                                // Почти полное совпадение текста
                                .Should( 
                                    sh => sh
                                    .MultiMatch( mm => mm
                                        .Query( query )
                                        .Type( TextQueryType.BestFields )
                                        .Operator( Operator.Or )
                                        .MinimumShouldMatch( "90%" )
                                        .Boost( 1 )
                                        .Fields( fs => fs 
                                            .Field( f => f.DataElements[ 0 ].Data )
                                            .Field( f => f.Title )
                                            .Field( f => f.Description )
                                            .Field( f => f.Url )
                                        )
                                    ),
                                    // Частичное совпадение текста
                                    sh => sh
                                    .MultiMatch( mm => mm
                                        .Query( query )
                                        .Type( TextQueryType.BestFields )
                                        .Operator( Operator.Or )
                                        .MinimumShouldMatch( "50%" )
                                        .Boost( 0.5 )
                                        .Fields( fs => fs
                                            .Field( f => f.DataElements[ 0 ].Data )
                                            .Field( f => f.Title )
                                            .Field( f => f.Description )
                                            .Field( f => f.Url )
                                        )
                                    ),
                                    // Добавление похожих результатов в поиск 
                                    sh => sh
                                    .MultiMatch( mm => mm
                                        .Query( query )
                                        .Type( TextQueryType.BestFields )
                                        .Operator( Operator.Or )
                                        .MinimumShouldMatch( "10%" )
                                        .Boost( 0.1 )
                                        .Fields( fs => fs
                                            .Field( f => f.DataElements[ 0 ].Data )
                                            .Field( f => f.Title )
                                            .Field( f => f.Description )
                                            .Field( f => f.Url )
                                        )
                                    )
                                )
                            )
                        )
                        // Увеличение счета, в зависимости от типа поискового элемента 
                        .Should( sh => sh
                            .Term( t => t
                                .Field( f => f.IsGenerated )
                                .Value( false )
                                .Boost( 50 )
                            )
                        )
                    )
                )
                // возможные предложения по автозаполнению текста
                .Suggest( suggest => suggest
                    .Completion( _suggestionName, c => c
                       .Prefix( query )
                       .Field( p => p.DataElements[ 0 ].Suggest )
                       .SkipDuplicates()
                       .Size( size )
                    )
                )
            );

            List<SearchResultDto>? searchResults = CreateSearchResults( response );
            List<SuggestResultDto>? suggestResults = CreateSuggestResults( response );

            return new ResultDto
            {
                SearchResults = searchResults,
                SuggestResults = suggestResults
            };
        }

        private List<SearchResultDto>? CreateSearchResults( ISearchResponse<PageDto> response )
        {
            return response
                .Hits
                .Select( result => new SearchResultDto
                {
                    Title = result.Source.Title,
                    Description = result.Source.Description,
                    Url = result.Source.Url,
                    Score = result.Score,
                } )
                .ToList();
        }

        private List<SuggestResultDto>? CreateSuggestResults( ISearchResponse<PageDto> response )
        {
            return response
                .Suggest[ _suggestionName ]
                .FirstOrDefault()
                ?.Options
                .Select( suggest => new SuggestResultDto
                {
                    Text = suggest.Text,
                    Score = suggest.Score,
                } )
                .ToList();
        }
    }
}
