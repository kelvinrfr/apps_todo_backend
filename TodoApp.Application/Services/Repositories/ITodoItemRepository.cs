using TodoApp.Domain.Todo;

namespace TodoApp.Application.Services.Repositories;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    Task<bool> UpdateStateAsync(long id, bool state, CancellationToken cancellation);
}
