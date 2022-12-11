using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoApp.Data.Database;
using TodoApp.Domain.Todo;
using TodoApp.Infrastructure.Database.Models;

namespace TodoApp.Repository
{
    public class TodoRepository : ITodoRepository
    {
        private readonly ILogger<TodoRepository> _logger;
        private readonly TodoAppDbContext _context;

        public TodoRepository(TodoAppDbContext context, ILogger<TodoRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TodoItem> GetAsync(long id)
        {
            _logger.LogDebug("Getting item '{id}'", id);
            var entity = await _context.TodoItems.FindAsync(id);
            return entity;
        }

        public async Task<IReadOnlyList<TodoItem>> List(string filter)
        {
            _logger.LogDebug("Getting items with filter '{filter}'", filter);
            var query = _context.TodoItems.AsQueryable();
            if (!string.IsNullOrWhiteSpace(filter))
            {
                query = query.Where(x => x.Description.Contains(filter));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> AddAsync(TodoItem item)
        {
            if (item == null)
            {
                _logger.LogError("TodoItem cannot be null");
                return false;
            }

            bool result = false;

            await _context.TodoItems.AddAsync(item);

            _logger.LogDebug("adding todo item '{description}'", item.Description);
            result = await _context.SaveChangesAsync() > 0;

            return result;
        }

        public async Task<bool?> UpdateAsync(long id, string description, bool state)
        {
            var entity = await _context.TodoItems.FindAsync(id);

            if (entity == null)
            {
                _logger.LogWarning("It was not possible to delete item '{id}' because it doesn't exist", id);
                return null;
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                entity.Description = description;
            }
            entity.State = state;

            _context.TodoItems.Update(entity);

            _logger.LogDebug("updating item '{id}'...", id);
            var affected = _context.SaveChanges();
            _logger.LogDebug("item '{id}' updated", id);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool?> UpdateStateAsync(long id, bool state)
        {
            return await UpdateAsync(id, description: null, state: state);
        }

        public async Task<bool?> DeleteAsync(long id)
        {
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null)
            {
                _logger.LogWarning("It was not possible to delete item '{id}' because it doesn't exist", id);
                return null;
            }
            _context.TodoItems.Remove(item);

            _logger.LogDebug("removing item '{id}'...", id);
            var affected = await _context.SaveChangesAsync();
            _logger.LogDebug("item '{id}' removed", id);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}