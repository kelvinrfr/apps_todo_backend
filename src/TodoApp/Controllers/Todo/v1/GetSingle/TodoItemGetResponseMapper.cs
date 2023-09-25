using AutoMapper;
using TodoApp.Api.Http.Controllers.Todo.v1;
using TodoApp.Domain.Todo;

namespace TodoApp.Api.Http.Controllers.Todo.v1.GetSingle
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
