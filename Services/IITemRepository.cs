using CodeZoneInventorySystem.Models;

namespace CodeZoneInventorySystem.Services
{
    public interface IITemRepository
    {
        public Item GetItem(int itemId);
        public void AddItem(Item item);
        public void EditItem(int itemId, Item updatedItem);
        public void DeleteItem(int itemId);
        public IEnumerable<Item> GetAllItems();

        public IEnumerable<string> GetAllStoresOfItem(int itemId);
    }
}
