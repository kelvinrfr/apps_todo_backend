using AutoMapper;
using TodoApp.Domain.Todo;

namespace TodoApp.Api.Http.WebApi.Controllers.Todo.v1.GetSingle
{
    public class TodoItemResponseBaseMapper : Profile
    {
        public TodoItemResponseBaseMapper()
        {
            CreateMap<TodoItem, TodoItemResponseBase>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.State));
        }
    }
}
