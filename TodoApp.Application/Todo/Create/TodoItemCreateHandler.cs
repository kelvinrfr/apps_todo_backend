using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Repository;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.Todo.Create
{
    public record TodoItemCreateRequest(string Description) : IRequest<bool>;

    internal class TodoItemCreateHandler : IRequestHandler<TodoItemCreateRequest, bool>
    {
        private readonly ILogger<TodoItemCreateHandler> _logger;
        private readonly ITodoRepository _store;

        public TodoItemCreateHandler(ILogger<TodoItemCreateHandler> logger, ITodoRepository store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<bool> Handle(TodoItemCreateRequest request, CancellationToken cancellationToken)
        {
            // TODO: cancellation token propagation
            if (request == null)
            {
                _logger.LogError("Request item cannot be null");
                return false;
            }

            var entity = new TodoItem
            {
                Description = request.Description,
                State = false
            };

            var result = await _store.AddAsync(entity);
            return result;
        }
    }
}
