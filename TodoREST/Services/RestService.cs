using HttpGenericRepository;
using TodoREST.Models;

namespace TodoREST.Services
{
    public class RestService : IRestService
    {
        private readonly IGenericRepository service;

        public RestService(IGenericRepository service)
        {
            this.service = service;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            var items = await service.GetAsync<List<Item>>(Constants.Endpoint);
            return items ?? [];
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            var item = await service.GetAsync<Item>($"{Constants.Endpoint}/{id}");
            return item ?? new Item();
        }

        public async Task SaveItemAsync(Item item, bool isNewItem = false)
        {
            if (isNewItem)
            {
                await service.PostAsync(Constants.Endpoint, item);
            }
            else
            {
                await service.PutAsync($"{Constants.Endpoint}/{item.Id}", item);
            }

            //Item newItem = await service.PostAsync<Item, Item>(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            bool result = await service.DeleteAsync($"{Constants.Endpoint}/{item.Id}");
        }
    }
}
