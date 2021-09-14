
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApp.Web.Controllers.v1;

namespace TodoApp.Service
{
    public interface ITodoService
    {
        Task<bool> CreateAsync(TodoItemCreateRequest request);
        IEnumerable<TodoItemResponse> List(string filter);
        Task<TodoItemResponse> GetAsync(long id);
        Task<bool?> UpdateStateAsync(long id, bool state);
        Task<bool?> UpdateAsync(long id, TodoItemPutRequest request);
        Task<bool?> DeleteAsync(long id);
    }
}