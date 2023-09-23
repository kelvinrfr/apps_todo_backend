using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.UseCases.Todo.Get;
using TodoApp.Api.Http.WebApi.Common;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoGetItemController : TodoJsonControllerBase
    {
        private readonly ILogger<TodoGetItemController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TodoGetItemController(ILogger<TodoGetItemController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }


        /// <summary>
        /// Get a specific todo item
        /// </summary>
        /// <param name="id">Todo item id</param>
        /// <returns>A specific todo item</returns>
        /// <response code="200">Returns a specific todo item</response>
        /// <response code="404">It was not possible to find the given item</response>
        /// <response code="400">Request has is not valid with the given parameters</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(TodoGetItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] long id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorDetailsResponse
                {
                    Error = "Id should be greater than zero."
                });
            }

            try
            {
                _logger.LogDebug("Getting item with id '{id}'", id);

                var entity = await _mediator.Send(new TodoGetSingleItemByIdRequest(id), cancellationToken);

                var response = _mapper.Map<TodoGetItemResponse>(entity);

                if (response != null)
                {
                    _logger.LogDebug("Item retrieved succesfully");
                    return Ok(response);
                }

                _logger.LogDebug("Todo item not found with the given id '{id}'", id);
                return NotFound();
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
