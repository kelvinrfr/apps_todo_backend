using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.UseCases.Todo.Update
{
    public record TodoItemUpdateRequest(long Id, string Description, bool State) : IRequest<bool>;

    internal class TodoItemUpdateHandler : IRequestHandler<TodoItemUpdateRequest, bool>
    {
        private readonly ILogger<TodoItemUpdateHandler> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoItemUpdateHandler(ILogger<TodoItemUpdateHandler> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(TodoItemUpdateRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError("Request item cannot be null");
                return false;
            }

            var entity = new TodoItem
            {
                Id = request.Id,
                Description = request.Description,
                State = request.State
            };

            var result = await _repository.UpdateAsync(entity, cancellationToken);
            return result;
        }
    }
}
