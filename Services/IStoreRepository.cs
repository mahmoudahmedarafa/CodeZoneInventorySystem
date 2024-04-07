using CodeZoneInventorySystem.Models;

namespace CodeZoneInventorySystem.Services
{
    public interface IStoreRepository
    {
        public Store GetStore(int storeId);
        public IEnumerable<Store> GetAllStores();
        public void AddStore(Store store);
        public void EditStore(int storeId, Store updatedStore);
        public void DeleteStore(int storeId);
    }
}
