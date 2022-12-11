using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.Application.Todo.ListItems;
using TodoApp.Domain.Todo;
using TodoApp.Repository;

namespace TodoApp.Application.Todo.GetSingle
{
    public record TodoGetSingleItemByIdRequest(long Id) : IRequest<TodoItem>;

    internal class TodoGetSingleItemByIdHandler : IRequestHandler<TodoGetSingleItemByIdRequest, TodoItem>
    {
        private readonly ILogger<TodoGetSingleItemByIdHandler> _logger;
        private readonly ITodoRepository _store;

        public TodoGetSingleItemByIdHandler(ILogger<TodoGetSingleItemByIdHandler> logger, ITodoRepository store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<TodoItem> Handle(TodoGetSingleItemByIdRequest request, CancellationToken cancellationToken)
        {
            // TODO: Cancellation token propagation
            var entity = await _store.GetAsync(request.Id);

            return entity;
        }
    }
}
