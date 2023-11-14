using HttpGenericRepository;
using TodoREST.Models;

namespace TodoREST.Services
{
    public class DataService : IDataService
    {
        private readonly IGenericRepository service;

        public DataService(IGenericRepository service)
        {
            this.service = service;
        }

        public async Task<List<Item>> GetItemsAsync()
        {
            UriBuilder builder = new(Constants.BaseUrl) { Path = Constants.Endpoint };
            return await service.GetAsync<List<Item>>(builder.Uri);
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            UriBuilder builder = new(Constants.BaseUrl) { Path = $"{Constants.Endpoint}/{id}" };
            return await service.GetAsync<Item>(builder.Uri);
        }

        public async Task SaveItemAsync(Item item, bool isNewItem = false)
        {
            if (isNewItem)
            {
                UriBuilder builder = new(Constants.BaseUrl) { Path = Constants.Endpoint };
                await service.PostAsync(builder.Uri, item);
            }
            else
            {
                UriBuilder builder = new(Constants.BaseUrl) { Path = $"{Constants.Endpoint}/{item.Id}" };
                await service.PutAsync(builder.Uri, item);
            }

            //Item newItem = await service.PostAsync<Item, Item>(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            UriBuilder builder = new(Constants.BaseUrl) { Path = $"{Constants.Endpoint}/{item.Id}" };
            bool result = await service.DeleteAsync(builder.Uri);
        }
    }
}
