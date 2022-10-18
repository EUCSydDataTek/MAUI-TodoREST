using TodoREST.Models;

namespace TodoREST.Services
{
    public interface ITodoService
    {
        Task<List<TodoItem>> GetTasksAsync();
        Task<TodoItem> GetTaskByIdAsync(string id);
        Task SaveTaskAsync(TodoItem item, bool isNewItem);
        Task DeleteTaskAsync(TodoItem item);
    }
}
