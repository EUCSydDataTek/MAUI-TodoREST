﻿using TodoAPI.Models;

namespace TodoAPI.Interfaces
{
    public interface ITodoRepository
    {
        bool DoesItemExist(string id);
        IEnumerable<Item> All { get; }
        Item? Find(string id);
        void Insert(Item item);
        void Update(Item item);
        void Delete(string id);
    }
}
