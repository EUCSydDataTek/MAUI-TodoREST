using ItemWebApi.Models;

namespace ItemWebApi.Services
{
    public class ItemService : IItemService
    {
        private List<Item>? _itemList;

        public ItemService()
        {
            InitializeData();
        }

        public IEnumerable<Item> GetAll()
        {
           return _itemList!;
        } 

        public bool DoesItemExist(string id)
        {
            return _itemList!.Any(item => item.ID == id);
        }

        public Item? Find(string id)
        {
            return _itemList!.FirstOrDefault(item => item.ID == id);
        }

        public void Insert(Item item)
        {
            item.ID = Guid.NewGuid().ToString();
            _itemList!.Add(item);
        }

        public void Update(Item inputItem)
        {
            var item = Find(inputItem.ID!);
            var index = _itemList!.IndexOf(item!);
            _itemList.RemoveAt(index);
            _itemList.Insert(index, item!);
        }

        public void Delete(string id)
        {
            _itemList!.Remove(Find(id)!);
        }

        private void InitializeData()
        {
            _itemList = new List<Item>
            {
                new Item
                {
                    ID = "6bb8a868-dba1-4f1a-93b7-24ebce87e243",
                    Name = "Learn app development",
                    Notes = "Take Microsoft Learn Courses",
                    Done = true
                },

                new Item
                {
                    ID = "b94afb54-a1cb-4313-8af3-b7511551b33b",
                    Name = "Develop apps",
                    Notes = "Use Visual Studio and Visual Studio for Mac",
                    Done = false
                },
                new Item
                {
                    ID = "ecfa6f80-3671-4911-aabe-63cc442c1ecf",
                    Name = "Publish apps",
                    Notes = "All app stores",
                    Done = false,
                }
            };
        }
    }
}
