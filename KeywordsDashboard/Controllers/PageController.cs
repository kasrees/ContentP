using Application.Commands;
using KeywordsDashboard.Dtos;
using KeywordsDashboard.Mappers;
using Microsoft.AspNetCore.Mvc;
using Application.Queries.Dtos;
using Application.Interfaces.Services;
using Application.Interfaces.Handlers;
using Microsoft.AspNetCore.Authorization;
using Domain.Entities;

namespace Api.Controllers
{
    [Route( "api/pages" )]
    [ApiController]
    [Authorize]
    public class PageController : ControllerBase
    {
        private readonly ICommandHandler<AddPageCommand, int?> _addPageHandler;
        private readonly ICommandHandler<DeletePageCommand, int?> _deletePageHandler;
        private readonly ICommandHandler<UpdatePageCommand, int?> _updatePageHandler;
        private readonly IPageService _pageService;

        public PageController(
            ICommandHandler<AddPageCommand, int?> addPageHandler,
            ICommandHandler<DeletePageCommand, int?> deletePageHandler,
            ICommandHandler<UpdatePageCommand, int?> updatePageHandler,
            IPageService pageService
        )
        {
            _addPageHandler = addPageHandler;
            _deletePageHandler = deletePageHandler;
            _updatePageHandler = updatePageHandler;
            _pageService = pageService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPageAsync( [FromBody] AddPageDto addPageDto)
        {
            int? pageId = await _addPageHandler.Handle(AddPageMapper.Map(addPageDto));
            return pageId != null
                ? Ok(pageId)
                : Conflict();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePageAsync(int id)
        {
            return await _deletePageHandler.Handle(new DeletePageCommand(id)) != null
                ? Accepted()
                : NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePageAsync(int id, [FromBody] UpdatePageDto updatePageDto)
        {
            return await _updatePageHandler.Handle(UpdatePageMapper.Map(id, updatePageDto)) != null
                ? Accepted()
                : NoContent();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPageDetailAsync(int id)
        {
            PageDetailWithAllTranslationsDto? page = await _pageService.GetPageDetailWithAllTranslationsAsync(id);
            return page is null
                ? NotFound()
                : Ok(page);
        }

        [HttpGet]
        public async Task<IActionResult> GetPageIndexAsync([FromQuery] GetPageIndexRequest getPageIndexRequest)
        {
            PageIndexDto? pageIndexDto = await _pageService.GetPageIndexAsync(GetPageIndexMapper.Map(getPageIndexRequest));
            return pageIndexDto is null
                ? NotFound()
                : Ok(pageIndexDto);
        }

        [Route("duplicates/{id}")]
        [HttpPost("{id}")]
        public IActionResult GetPageTitleDuplicates(int id, [FromBody] GetPageTitleDuplicates getPageByTitleRequest)
        {
            List<int> languagesId = _pageService.GetLanguagesOfTitleDuplicates(new GetPageTitleDuplicatesRequestDto
            {
                PageId = id,
                Titles = getPageByTitleRequest.Titles,
            });
            return languagesId.Count() == 0
                ? NotFound()
                : Ok(languagesId);
        }
    }
}
