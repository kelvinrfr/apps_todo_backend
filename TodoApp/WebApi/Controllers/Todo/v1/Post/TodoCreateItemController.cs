using System;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoApp.Application.UseCases.Todo.Create;
using TodoApp.WebApi.Common;

namespace TodoApp.WebApi.Controllers.Todo.v1.Post
{
    public abstract class TodoCreateItemController : TodoJsonControllerBase
    {
        private readonly ILogger<TodoCreateItemController> _logger;
        private readonly IMediator _mediator;

        public TodoCreateItemController(ILogger<TodoCreateItemController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
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
        public async Task<IActionResult> Create([FromBody] TodoCreateItemRequest request)
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
                bool result = await _mediator.Send(new TodoItemCreateRequest(request.Description));

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
    }
}
