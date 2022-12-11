using System.Collections.Generic;

namespace TodoApp.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoGetListItemResponse : TodoItemResponseBase
    { }

    public class TodoGetListResponse
    {
        public IReadOnlyList<TodoGetListItemResponse> Data { get; set; }
    }
}
