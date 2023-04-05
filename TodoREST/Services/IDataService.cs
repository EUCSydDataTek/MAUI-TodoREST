using TodoREST.Models;

namespace TodoREST.Services
{
    public interface IDataService
    {
        Task<List<Item>> GetItemsAsync();
        Task<Item> GetItemByIdAsync(string id);
        Task SaveItemAsync(Item item, bool isNewItem);
        Task DeleteItemAsync(Item item);
    }
}
