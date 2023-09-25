using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.UseCases.Todo.Update;
using TodoApp.Api.Http.Common;

namespace TodoApp.Api.Http.Controllers.Todo.v1.Patch
{
	public class TodoPatchItemController : TodoJsonControllerBase
	{
		private readonly ILogger<TodoPatchItemController> _logger;
		private readonly IMediator _mediator;

		public TodoPatchItemController(ILogger<TodoPatchItemController> logger, IMediator mediator)
		{
			_logger = logger;
			_mediator = mediator;
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
		public async Task<IActionResult> ChangeState([FromRoute] long id, [FromBody] bool state, CancellationToken cancellationToken)
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

				var result = await _mediator.Send(new TodoItemUpdateStateRequest(id, state), cancellationToken);

				if (result == true)
				{
					_logger.LogDebug("Todo item '{id}' updated successfully", id);
					return NoContent();
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
