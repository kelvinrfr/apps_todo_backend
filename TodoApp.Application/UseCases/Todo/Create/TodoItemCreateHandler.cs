using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Domain.Todo;
using TodoApp.Application.Services.Repositories;

namespace TodoApp.Application.UseCases.Todo.Create
{
    public record TodoItemCreateRequest(string Description) : IRequest<bool>;

    internal class TodoItemDeleteHandler : IRequestHandler<TodoItemCreateRequest, bool>
    {
        private readonly ILogger<TodoItemDeleteHandler> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoItemDeleteHandler(ILogger<TodoItemDeleteHandler> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(TodoItemCreateRequest request, CancellationToken cancellationToken)
        {
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

            var result = await _repository.AddAsync(entity, cancellationToken);
            return result;
        }
    }
}
