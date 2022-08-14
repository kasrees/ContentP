using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Services;
using Application.Dto;

namespace FullTextExtranetSearch.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searcher;

        public SearchController( ISearchService searcher )
        {
            _searcher = searcher;
        }
        /// <summary>
        /// Search extranet menu page by query
        /// </summary>>
        /// <returns></returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /search/search-by-query
        ///     {
        ///        "query": "tariffs",
        ///        "limit": 3
        ///     }
        ///
        /// </remarks>
        /// <response code="200">The request executed successfully</response>
        /// <response code="400">Invalid request</response>
        // POST: /<SearchController>
        [HttpPost("search-by-query")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string[]))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json", Type = typeof(string[]))]
        //[ApiKeyAuthorize("X-API-KEY", "1cf2c8ce-d81a-11ec-9d64-0242ac120002")]
        public IActionResult SearchByQuery( [FromBody] Api.Models.SearchRequest searchRequest )
        {
            ResultDto? result = _searcher.Search( searchRequest.Query, searchRequest.Limit );
            return Ok( result );
        }
    }
}
