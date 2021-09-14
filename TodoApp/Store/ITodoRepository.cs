using System.Linq;
using System.Threading.Tasks;
using TodoApp.Data.Models;

namespace TodoApp.Repository
{
    public interface ITodoRepository 
    {
        Task<TodoItem> GetAsync(long id);
        IQueryable<TodoItem> List(string descriptionFilter);
        Task<bool> AddAsync(TodoItem item);
        Task<bool?> UpdateAsync(long id, string description, bool state);

        Task<bool?> UpdateStateAsync(long id, bool state);
        Task<bool?> DeleteAsync(long id);
    }
}