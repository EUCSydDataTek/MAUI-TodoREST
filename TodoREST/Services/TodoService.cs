using TodoREST.Models;

namespace TodoREST.Services
{
    public class TodoService(IRestService service) : ITodoService
    {
        public Task<List<TodoItem>> GetTasksAsync()
        {
            return _restService.RefreshDataAsync();
        }

        public Task SaveTaskAsync(TodoItem item, bool isNewItem = false)
        {
            return _restService.SaveTodoItemAsync(item, isNewItem);
        }

        public Task DeleteTaskAsync(TodoItem item)
        {
            return _restService.DeleteTodoItemAsync(item.Id);
        }
    }
}
