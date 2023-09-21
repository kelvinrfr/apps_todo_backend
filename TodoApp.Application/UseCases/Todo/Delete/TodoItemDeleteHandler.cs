using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;

namespace TodoApp.Application.UseCases.Todo.Delete
{
    public record TodoItemDeleteRequest(long Id) : IRequest<bool>;

    internal class TodoItemDeleteHandler : IRequestHandler<TodoItemDeleteRequest, bool>
    {
        private readonly ILogger<TodoItemDeleteHandler> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoItemDeleteHandler(ILogger<TodoItemDeleteHandler> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(TodoItemDeleteRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError("Request item cannot be null");
                return false;
            }

            var result = await _repository.DeleteAsync(request.Id, cancellationToken);
            return result;
        }
    }
}
