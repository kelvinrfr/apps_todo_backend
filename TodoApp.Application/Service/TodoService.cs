using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TodoApp.Data.Models;
using TodoApp.Repository;
using TodoApp.Web.Controllers.v1;
using System.Linq;

namespace TodoApp.Service
{
    public class TodoService : ITodoService
    {
        private readonly ILogger<TodoService> _logger;
        private readonly ITodoRepository _store;
        public TodoService(ILogger<TodoService> logger, ITodoRepository store)
        {
            _logger = logger;
            _store = store;
        }

        public async Task<bool> CreateAsync(TodoItemCreateRequest request)
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

            var result = await _store.AddAsync(entity);
            return result;
        }

        public async Task<bool?> DeleteAsync(long id)
        {
            return await _store.DeleteAsync(id);
        }

        public async Task<TodoItemResponse> GetAsync(long id)
        {
            TodoItemResponse response = null;
            var entity = await _store.GetAsync(id);

            if (entity != null)
            {
                response = new TodoItemResponse
                {
                    Id = entity.Id,
                    Description = entity.Description,
                    State = entity.State
                };
            }
            return response;
        }

        public IEnumerable<TodoItemResponse> List(string filter)
        {
            var todoItems = _store.List(filter);

            var result = todoItems.Select(x => new TodoItemResponse
            {
                Id = x.Id,
                Description = x.Description,
                State = x.State
            }).AsEnumerable();

            return result;
        }

        public async Task<bool?> UpdateAsync(long id, TodoItemPutRequest request)
        {
            return await _store.UpdateAsync(id, request.Description, request.State);
        }

        public async Task<bool?> UpdateStateAsync(long id, bool state)
        {
            return await _store.UpdateStateAsync(id, state);
        }
    }
}