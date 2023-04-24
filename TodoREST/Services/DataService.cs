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
            Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
            return await service.GetAsync<List<Item>>(uri);
        }

        public async Task<Item> GetItemByIdAsync(string id)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, id));
            return await service.GetAsync<Item>(uri);
        }

        public async Task SaveItemAsync(Item item, bool isNewItem = false)
        {


            if (isNewItem)
            {
                Uri uri = new Uri(string.Format(Constants.RestUrl, string.Empty));
                await service.PostAsync(uri, item);
            }
            else
            {
                Uri uri = new Uri(string.Format(Constants.RestUrl, item.Id));
                await service.PutAsync(uri, item);
            }

            //Item newItem = await service.PostAsync<Item, Item>(item);
        }

        public async Task DeleteItemAsync(Item item)
        {
            Uri uri = new Uri(string.Format(Constants.RestUrl, item.Id));
            bool result = await service.DeleteAsync(uri);
        }
    }
}
