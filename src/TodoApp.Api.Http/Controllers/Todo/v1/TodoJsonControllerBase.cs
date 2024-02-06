using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Api.Http.Controllers.Todo.v1
{
	[ApiController]
	[Route("v1/todo")]
	[Produces("application/json")]
	public abstract class TodoJsonControllerBase : ControllerBase
	{
		protected const string V1TodoTag = "V1 Todo";
	}
}
