using AutoMapper;
using System.Collections.Generic;
using TodoApp.Domain.Todo;

namespace TodoApp.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoGetListResponseMapper : Profile
    {
        public TodoGetListResponseMapper()
        {
            CreateMap<TodoItem, TodoGetListItemResponse>()
                .IncludeBase<TodoItem, TodoItemResponseBase>();

            CreateMap<IEnumerable<TodoItem>, TodoGetListResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src));
        }
    }
}
