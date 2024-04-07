using CodeZoneInventorySystem.Models;
using CodeZoneInventorySystem.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CodeZoneInventorySystem.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext context;

        public StoreRepository(AppDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Store GetStore(int storeId)
        {
            return context.Stores.Where(st => st.Id == storeId)
                                .Include(st => st.StoreItems)
                                .FirstOrDefault();
        }

        public void AddStore(Store store)
        {
            context.Stores.Add(store);
            context.SaveChanges();
        }

        public void EditStore(int storeId, Store updatedStore)
        {
            var store = context.Stores.Where(st => st.Id == storeId).FirstOrDefault();

            if(store != null)
            {
                store.Name = updatedStore.Name;
                store.Address = updatedStore.Address;

                context.SaveChanges();
            }
        }

        public void DeleteStore(int storeId)
        {
            var store = context.Stores.Where(st => st.Id == storeId).FirstOrDefault();

            if (store != null)
            {
                context.Remove(store.StoreItems);

                context.Remove(store);

                context.SaveChanges();
            }
        }

        public IEnumerable<Store> GetAllStores()
        {
            var allStores = context.Stores.ToList();

            return allStores;
        }
    }
}
