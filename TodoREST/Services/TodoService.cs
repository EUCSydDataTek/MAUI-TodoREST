using HttpGenericRepository;
using TodoREST.Models;

namespace TodoREST.Services
{
    public class TodoService : ITodoService
    {
        private readonly IGenericRepository service;

        public TodoService(IGenericRepository service)
        {
            this.service = service;
        }

        public async Task<List<TodoItem>> GetTasksAsync()
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            return await service.GetAsync<List<TodoItem>>(uri);
        }

        public async Task<TodoItem> GetTaskByIdAsync(string id)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, id));
            return await service.GetAsync<TodoItem>(uri);
        }

        public async Task SaveTaskAsync(TodoItem item, bool isNewItem = false)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));

            if (isNewItem)
                await service.PostAsync(uri, item);
            else
                await service.PutAsync(uri, item);

            //TodoItem newTodoItem = await service.PostAsync<TodoItem, TodoItem>(item);
        }

        public async Task DeleteTaskAsync(TodoItem item)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, item.ID));
            bool result = await service.DeleteAsync(uri);
        }
    }
}
