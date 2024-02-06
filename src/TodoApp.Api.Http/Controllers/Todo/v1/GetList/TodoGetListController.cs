using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.UseCases.Todo.List;
using TodoApp.Api.Http.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace TodoApp.Api.Http.Controllers.Todo.v1.GetList
{
	public class TodoGetListController : TodoJsonControllerBase
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
		[SwaggerOperation(Tags = [V1TodoTag])]
		[ProducesResponseType(typeof(IEnumerable<TodoGetListItemResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ErrorDetailsResponse), StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> Get([FromQuery] string filter, CancellationToken cancellationToken)
		{
			try
			{
				_logger.LogDebug("Getting items with filter '{filter}'", filter);

				IEnumerable<Domain.Todo.TodoItem> entities = await _mediator.Send(new TodoItemListItemsByDescriptionRequest(filter), cancellationToken);

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
