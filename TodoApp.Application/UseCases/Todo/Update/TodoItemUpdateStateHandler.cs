using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;

namespace TodoApp.Application.UseCases.Todo.Update
{
    public record TodoItemUpdateStateRequest(long Id, bool State) : IRequest<bool>;

    internal class TodoItemUpdateStateHandler : IRequestHandler<TodoItemUpdateStateRequest, bool>
    {
        private readonly ILogger<TodoItemUpdateStateHandler> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoItemUpdateStateHandler(ILogger<TodoItemUpdateStateHandler> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<bool> Handle(TodoItemUpdateStateRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                _logger.LogError("Request item cannot be null");
                return false;
            }

            var result = await _repository.UpdateStateAsync(request.Id, request.State, cancellationToken);
            return result;
        }
    }
}
