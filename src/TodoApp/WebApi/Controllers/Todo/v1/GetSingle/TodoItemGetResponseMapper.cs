using AutoMapper;
using TodoApp.Domain.Todo;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoItemGetResponseMapper : Profile
    {
        public TodoItemGetResponseMapper()
        {
            CreateMap<TodoItem, TodoGetItemResponse>()
                .IncludeBase<TodoItem, TodoItemResponseBase>();
        }
    }
}
