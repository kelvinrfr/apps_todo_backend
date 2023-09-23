using MediatR;
using Microsoft.Extensions.Logging;
using TodoApp.Application.Services.Repositories;
using TodoApp.Domain.Todo;

namespace TodoApp.Application.UseCases.Todo.Get
{
    public record TodoGetSingleItemByIdRequest(long Id) : IRequest<TodoItem>;

    internal class TodoGetSingleItemByIdHandler : IRequestHandler<TodoGetSingleItemByIdRequest, TodoItem>
    {
        private readonly ILogger<TodoGetSingleItemByIdHandler> _logger;
        private readonly ITodoItemRepository _repository;

        public TodoGetSingleItemByIdHandler(ILogger<TodoGetSingleItemByIdHandler> logger, ITodoItemRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task<TodoItem> Handle(TodoGetSingleItemByIdRequest request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Getting item with id {id}", request?.Id);

            if (request == null)
            {
                throw new ArgumentException("Invalid param.", nameof(request));
            }

            var entity = await _repository.GetAsync(request.Id, cancellationToken);

            return entity;
        }
    }
}
