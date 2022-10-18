using TodoREST.Models;
using TodoREST.Repository;

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
            return await service.GetAsync<List<TodoItem>>();
        }

        public async Task<TodoItem> GetTaskByIdAsync(string id)
        {
            TodoItem item = await service.GetAsync<TodoItem>(id);
            return item;
        }

        public async Task SaveTaskAsync(TodoItem item, bool isNewItem = false)
        {
            if (isNewItem)
                await service.PostAsync(item);
            else
                await service.PutAsync(item);


            //TodoItem newTodoItem = await service.PostAsync<TodoItem, TodoItem>(item);
        }

        public Task DeleteTaskAsync(TodoItem item)
        {
            return service.DeleteAsync(item.ID);
        }
    }
}
