using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TodoApp.Application.Services.Repositories;
using TodoApp.Data.Database;
using TodoApp.Domain.Todo;

namespace TodoApp.Infrastructure.Database.Repository
{
    internal class TodoItemRepository : ITodoItemRepository
    {
        private readonly ILogger<TodoItemRepository> _logger;
        private readonly TodoAppDbContext _context;

        public TodoItemRepository(TodoAppDbContext context, ILogger<TodoItemRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<TodoItem> GetAsync(long id, CancellationToken cancellation)
        {
            _logger.LogDebug("Getting item '{id}'", id);
            var entity = await _context.TodoItems.FindAsync(id, cancellation);
            return entity;
        }

        public async Task<IReadOnlyList<TodoItem>> ListAsync(Expression<Func<TodoItem, bool>> predicate, CancellationToken cancellation)
        {
            _logger.LogDebug("Getting items with filter '{filter}'", predicate.Body.ToString());
            var query = _context.TodoItems
                .AsNoTracking()
                .AsQueryable()
                .Where(predicate);

            return await query.ToListAsync(cancellation);
        }

        public async Task<bool> AddAsync(TodoItem entity, CancellationToken cancellation)
        {
            if (entity == null)
            {
                _logger.LogError("TodoItem cannot be null");
                return false;
            }

            bool result = false;

            await _context.TodoItems.AddAsync(entity, cancellation);

            _logger.LogDebug("adding todo item '{description}'", entity.Description);
            result = await _context.SaveChangesAsync(cancellation) > 0;

            return result;
        }

        public async Task<bool> UpdateAsync(TodoItem entity, CancellationToken cancellation)
        {
            if (entity == null || entity.Id == default)
            {
                _logger.LogError("Update was not possible because no Entity ID was provided");
                return false;
            }

            _context.TodoItems.Update(entity);

            _logger.LogDebug("updating item '{id}'...", entity.Id);
            var affected = await _context.SaveChangesAsync(cancellation);
            _logger.LogDebug("item '{id}' updated", entity.Id);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateStateAsync(long id, bool state, CancellationToken cancellation)
        {
            if (id == default)
            {
                _logger.LogError("Update was not possible because no Entity ID was provided");
                return false;
            }

            var entity = new TodoItem { Id = id, State = state };

            _context.TodoItems.Attach(entity);
            _context.Entry(entity).Property(p => p.State).IsModified = true;

            _logger.LogDebug("updating item '{id}'...", entity.Id);
            var affected = await _context.SaveChangesAsync(cancellation);
            _logger.LogDebug("item '{id}' updated", entity.Id);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> DeleteAsync(long id, CancellationToken cancellation)
        {
            var item = new TodoItem
            {
                Id = id
            };

            _context.TodoItems.Remove(item);

            _logger.LogDebug("removing item '{id}'...", id);
            var affected = await _context.SaveChangesAsync(cancellation);
            _logger.LogDebug("item '{id}' removed", id);

            if (affected == 0)
            {
                return false;
            }

            return true;
        }
    }
}