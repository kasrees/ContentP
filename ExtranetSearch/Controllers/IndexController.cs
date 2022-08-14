using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FullTextExtranetSearch.Controllers
{
    [ApiController]
    [Route( "api/[controller]" )]
    public class IndexController : ControllerBase
    {
        private readonly IIndexSynchronizationService _synchronizer;

        public IndexController( IIndexSynchronizationService synchronizer )
        {
            _synchronizer = synchronizer;
        }

        [HttpGet]
        [Route( "index" )]
        public async Task<IActionResult> IndexAsync()
        {
            await _synchronizer.SynchronizeAsync();
            return Ok();
        }
    }
}
