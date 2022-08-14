using Application.Interfaces.Services;
using Application.Models;
using Application.OptionModels;
using Nest;

namespace Application.Services
{
    public class IndexCreationService : IIndexCreationService
    {
        private readonly IElasticClient _client;

        public IndexCreationService( IElasticClient client )
        {
            _client = client;
        }

        public async Task CreateIndexIfDoesNotExistAsync( IndexConfig options )
        {
            if ( _client.Indices.Exists( options.IndexName ).Exists )
            {
                await _client.Indices.CreateAsync( options.IndexName,
                index => index.Map<Models.Page>( x => x
                    .AutoMap()
                    .Properties( prop => prop
                        .Object<DataElement>( o => o
                            .Name( n => n.DataElements )
                            .Properties( elementProp => elementProp
                                .Text( t => t
                                    .Name( data => data.Data )
                                    .Analyzer( options.Analyzer )
                                    .Fields( fl => fl
                                        .Keyword( kw => kw
                                            .Name( "keyword" )
                                            .IgnoreAbove( 256 )
                                        )
                                    )
                                )
                                .Text( t => t
                                    .Name( tag => tag.Tag )
                                    .Fields( fl => fl
                                        .Keyword( kw => kw
                                            .Name( "keyword" )
                                            .IgnoreAbove( 256 )
                                        )
                                    )
                                )
                                .Number( n => n
                                    .Name( weight => weight.Weight )
                                    .Type( NumberType.Integer )
                                )
                                .Completion( comp => comp
                                    .Name( suggest => suggest.Suggest )
                                    .Analyzer( "simple" )
                                )
                            )
                        )
                    )
                    )
                );
            }
        }

        public async Task DeleteIndexIfExistsAsync( IndexConfig options )
        {
            if ( _client.Indices.Exists( options.IndexName ).Exists )
            {
                await _client.Indices.DeleteAsync( options.IndexName );
            }
        }
    }
}
