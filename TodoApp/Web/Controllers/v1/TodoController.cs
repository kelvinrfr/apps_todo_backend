using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Service;
using TodoApp.Web.Models;

namespace TodoApp.Web.Controllers.v1
{
    [ApiController]
    [Route("v1/todo")]
    [Produces("application/json")]
    public class TodoController : ControllerBase
    {
        private readonly ILogger<TodoController> _logger;
        private readonly ITodoService _todoService;

        public TodoController(ILogger<TodoController> logger, ITodoService todoService)
        {
            _logger = logger;
            _todoService = todoService;
        }

        /// <summary>
        /// Get all Todo items
        /// </summary>
        /// <param name="filter">Optional filter to be applied</param>
        /// <returns>List of all todo items</returns>
        /// <response code="200">Returns the list of todo items</response>
        /// <response code="500">Unexpected error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult Get([FromQuery] string filter)
        {
            try
            {
                _logger.LogDebug("Getting items with filter '{filter}'", filter);
                IEnumerable<TodoItemResponse> response = _todoService.List(filter);

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
        [ProducesResponseType(typeof(TodoItemResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromRoute] long id)
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
                TodoItemResponse response = await _todoService.GetAsync(id);

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

        /// <summary>
        /// Add a new Todo item
        /// </summary>
        /// <param name="request">Todo item to be created</param>
        /// <returns>Returns a successfull status code if operation succeed</returns>
        /// <response code="201">Successfully created a todo item</response>
        /// <response code="400">Request has is not valid with the given parameters</response>
        /// <response code="500">Unexpected error</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] TodoItemCreateRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Description))
            {
                return BadRequest(new ErrorDetailsResponse
                {
                    Error = "Invalid request with the given parameters"
                });
            }

            try
            {
                _logger.LogDebug("Creating a todo item '{description}'", request.Description);
                bool result = await _todoService.CreateAsync(request);

                if (result)
                {
                    _logger.LogDebug("Todo item '{description}' created successfully", request.Description);
                    return StatusCode(StatusCodes.Status201Created);
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

        /// <summary>
        /// Change state of a todo item
        /// </summary>
        /// <param name="id">Todo item id to be updated</param>
        /// <param name="state">Todo item new state</param>
        /// <returns>Returns a successfull status code if operation succeed</returns>
        /// <response code="204">Successfully update a todo item state</response>
        /// <response code="400">Request has is not valid with the given parameters</response>
        /// <response code="404">It was not possible to find the given item</response>
        /// <response code="500">Unexpected error</response>
        [HttpPatch]
        [Route("{id}/state")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeState([FromRoute] long id, [FromBody] bool state)
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
                _logger.LogDebug("Updating todo item '{id}' state to '{state}'", id, state);
                bool? result = await _todoService.UpdateStateAsync(id, state);

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
        public async Task<IActionResult> Change([Required][FromRoute] long id, [FromBody] TodoItemPutRequest request)
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
                bool? result = await _todoService.UpdateAsync(id, request);

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
