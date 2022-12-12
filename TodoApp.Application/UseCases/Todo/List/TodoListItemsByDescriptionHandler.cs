using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.UseCases.Todo.List
{
    public record TodoItemListItemsByDescriptionRequest(string Description) : IRequest<IEnumerable<TodoItem>>;

    internal class TodoListItemsByDescriptionHandler : IRequestHandler<TodoItemListItemsByDescriptionRequest, IEnumerable<TodoItem>>
    {
        private readonly ILogger<TodoItemListItemsByDescriptionRequest> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoListItemsByDescriptionHandler(ILogger<TodoItemListItemsByDescriptionRequest> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IEnumerable<TodoItem>> Handle(TodoItemListItemsByDescriptionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching the list of items with the request {request}", request);

            var todoItems = await _repository.ListAsync(todo =>
                todo.Description != null && todo.Description.Contains(request.Description) == true,
                cancellationToken);

            return todoItems.AsEnumerable();
        }
    }
}
