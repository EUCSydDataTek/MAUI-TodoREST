using ItemWebApi.Models;

namespace ItemWebApi.Services
{
    public interface IItemService
    {
        bool DoesItemExist(string id);
        IEnumerable<Item> GetAll();
        Item? Find(string id);
        void Insert(Item item);
        void Update(Item item);
        void Delete(string id);
    }
}
