using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1
{
    [ApiController]
    [Route("v1/todo")]
    [Produces("application/json")]
    public abstract class TodoJsonControllerBase : ControllerBase
    { }
}
