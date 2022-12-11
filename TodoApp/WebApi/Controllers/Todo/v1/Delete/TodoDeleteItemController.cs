using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.WebApi.Common;

namespace TodoApp.WebApi.Controllers.Todo.v1.Delete
{
    public abstract class TodoDeleteItemController : TodoJsonControllerBase
    {
        private readonly ILogger<TodoDeleteItemController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public TodoDeleteItemController(ILogger<TodoDeleteItemController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Change state of a todo item
        /// </summary>
        /// <param name="id">Todo item id to be deleted</param>
        /// <returns>Returns a successfull status code if operation succeed</returns>
        /// <response code="204">Successfully update a todo item state</response>
        /// <response code="404">It was not possible to find the given item</response>
        /// <response code="500">Unexpected error</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorDetailsResponse
                {
                    Error = "Id should be greater than zero"
                });
            }

            try
            {
                _logger.LogDebug("Deleting todo item '{id}'", id);
                bool? result = await _todoService.DeleteAsync(id);

                if (result == true)
                {
                    _logger.LogDebug("Todo item '{id}' deleted successfully", id);
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
