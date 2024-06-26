﻿using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Services;
using Microsoft.EntityFrameworkCore;
using static System.Formats.Asn1.AsnWriter;

namespace CodeZoneInventorySystem.Repositories
{
    public class ItemRepository : IITemRepository
    {
        private readonly AppDbContext context;

        public ItemRepository(AppDbContext context) 
        {
            this.context = context;
        }

        public Item GetItem(int itemId)
        {
            return context.Items.Where(item => item.Id == itemId)
                                .Include(st => st.StoreItems)
                                .FirstOrDefault();
        }

        public void AddItem(Item item)
        {
            context.Items.Add(item);
            context.SaveChanges();
        }

        public void EditItem(int itemId, Item updatedItem)
        {
            var item = context.Items.Where(item => item.Id == itemId).FirstOrDefault();

            if (item != null)
            {
                item.Name = updatedItem.Name;
                item.Description = updatedItem.Description;

                context.SaveChanges();
            }
        }

        public void DeleteItem(int itemId)
        {
            var item = context.Items.Where(item => item.Id == itemId)
                                    .Include(item => item.StoreItems)
                                    .FirstOrDefault();

            if(item != null)
            {
                foreach(var stItem in item.StoreItems)
                {
                    context.StoreItems.Remove(stItem);
                }


                context.Remove(item);

                context.SaveChanges();
            }
        }

        public IEnumerable<Item> GetAllItems()
        {
            var allItems = context.Items.ToList();

            return allItems;
        }

        public IEnumerable<string> GetAllStoresOfItem(int itemId)
        {
            var storeNames = context.StoreItems.Where(stItem => stItem.ItemId == itemId)
                                               .Include(stItem => stItem.Store)
                                               .Select(store => store.Store)
                                               .Select(name => name.Name)
                                               .ToList();

            return storeNames;
        }
    }
}
