using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.UseCases.Todo.Update;
using TodoApp.Api.Http.WebApi.Common;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1.Put
{
    public  class TodoPutItemController : TodoJsonControllerBase
    {
        private readonly ILogger<TodoPutItemController> _logger;
        private readonly IMediator _mediator;

        public TodoPutItemController(ILogger<TodoPutItemController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// Change state of a todo item
        /// </summary>
        /// <param name="id">Todo item id to be updated</param>
        /// <param name="request">Updated Todo item</param>
        /// <returns>Returns a successfull status code if operation succeed</returns>
        /// <response code="204">Successfully update a todo item</response>
        /// <response code="400">Request has is not valid with the given parameters</response>
        /// <response code="404">It was not possible to find the given item</response>
        /// <response code="500">Unexpected error</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Change([Required][FromRoute] long id, [FromBody] TodoPutItemRequest request, CancellationToken cancellationToken)
        {
            if (id <= 0 || request == null || request.Description == null)
            {
                return BadRequest(new ErrorDetailsResponse
                {
                    Error = "Id should be greater than zero and request body should be filled."
                });
            }

            try
            {
                _logger.LogDebug("Updating todo item '{id}'", id);
                bool? result = await _mediator.Send(new TodoItemUpdateRequest(id, request.Description, request.State), cancellationToken);

                if (result == true)
                {
                    _logger.LogDebug("Todo item '{id}' updated successfully", id);
                    return NoContent();
                }
                else if (result == null)
                {
                    _logger.LogDebug("Todo item not found with the given id '{id}'", id);
                    return NotFound();
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
