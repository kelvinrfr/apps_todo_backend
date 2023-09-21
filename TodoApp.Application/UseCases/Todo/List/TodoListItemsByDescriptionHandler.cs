using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.UseCases.Todo.List
{
    public record TodoItemListItemsByDescriptionRequest(string Description) : IRequest<IReadOnlyList<TodoItem>>;

    internal class TodoListItemsByDescriptionHandler : IRequestHandler<TodoItemListItemsByDescriptionRequest, IReadOnlyList<TodoItem>>
    {
        private readonly ILogger<TodoItemListItemsByDescriptionRequest> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoListItemsByDescriptionHandler(ILogger<TodoItemListItemsByDescriptionRequest> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<IReadOnlyList<TodoItem>> Handle(TodoItemListItemsByDescriptionRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching the list of items with the request {request}", request);

            if (request.Description != null)
            {
                return await _repository.ListAsync(todo => todo.Description!.Contains(request.Description) == true, cancellationToken);
            }

            return await _repository.ListAsync(cancellationToken);

        }
    }
}
