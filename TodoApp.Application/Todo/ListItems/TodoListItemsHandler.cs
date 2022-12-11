using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Repository;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.Todo.ListItems
{
    public record TodoItemListItemsRequest(string Filter) : IRequest<IEnumerable<TodoItem>>;

    internal class TodoListItemsHandler : IRequestHandler<TodoItemListItemsRequest, IEnumerable<TodoItem>>
    {
        private readonly ILogger<TodoItemListItemsRequest> _logger;
        private readonly ITodoRepository _store;

        public TodoListItemsHandler(ILogger<TodoItemListItemsRequest> logger, ITodoRepository store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<IEnumerable<TodoItem>> Handle(TodoItemListItemsRequest request, CancellationToken cancellationToken)
        {
            var todoItems = await _store.List(request.Filter);

            //var result = todoItems.Select(x => new TodoItemResponse
            //{
            //    Id = x.Id,
            //    Description = x.Description,
            //    State = x.State
            //}).AsEnumerable();

            return todoItems.AsEnumerable();
        }
    }
}
