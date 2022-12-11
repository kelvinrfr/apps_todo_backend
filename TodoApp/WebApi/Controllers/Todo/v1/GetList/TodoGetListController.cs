using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Todo.ListItems;
using TodoApp.Service;
using TodoApp.WebApi.Common;
using TodoApp.WebApi.Controllers.Todo.v1.GetSingle;

namespace TodoApp.WebApi.Controllers.Todo.v1.GetList
{
    public abstract class TodoGetListController: TodoControllerBase
    {
        private readonly ILogger<TodoGetListController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TodoGetListController(ILogger<TodoGetListController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all Todo items
        /// </summary>
        /// <param name="filter">Optional filter to be applied</param>
        /// <returns>List of all todo items</returns>
        /// <response code="200">Returns the list of todo items</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoGetListItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery] string filter)
        {
            try
            {
                _logger.LogDebug("Getting items with filter '{filter}'", filter);

                IEnumerable<Domain.Todo.TodoItem> entities = await _mediator.Send(new TodoItemListItemsRequest(filter));

                var response = _mapper.Map<TodoGetListResponse>(entities);

                if (response != null)
                {
                    _logger.LogDebug("Items retrieved succesfully");
                    return Ok(response);
                }
                _logger.LogWarning("Didn't get an expected response from service.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error has occurred");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new ErrorDetailsResponse
            {
                Error = "An unexpected error has occurred"
            });
        }
    }
}
