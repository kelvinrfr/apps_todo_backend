using System.Collections.Generic;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1.GetList
{
    public class TodoGetListItemResponse : TodoItemResponseBase
    { }

    public class TodoGetListResponse
    {
        public IReadOnlyList<TodoGetListItemResponse> Data { get; set; }
    }
}
