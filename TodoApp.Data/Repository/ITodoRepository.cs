using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApp.Domain.Todo;

namespace TodoApp.Repository
{
    public interface ITodoRepository 
    {
        Task<TodoItem> GetAsync(long id);
        Task<IReadOnlyList<TodoItem>> List(string descriptionFilter);
        Task<bool> AddAsync(TodoItem item);
        Task<bool?> UpdateAsync(long id, string description, bool state);

        Task<bool?> UpdateStateAsync(long id, bool state);
        Task<bool?> DeleteAsync(long id);
    }
}