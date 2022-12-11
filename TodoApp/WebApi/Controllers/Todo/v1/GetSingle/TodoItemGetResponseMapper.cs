using AutoMapper;
using TodoApp.Domain.Todo;

namespace TodoApp.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoGetItemMappers : Profile
    {
        public TodoGetItemMappers()
        {
            CreateMap<TodoItem, TodoGetItemResponse>()
                .IncludeBase<TodoItem, TodoItemResponseBase>();
        }
    }
}
