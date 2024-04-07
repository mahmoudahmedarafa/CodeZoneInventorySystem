using System;
using System.Collections.Generic;

namespace CodeZoneInventorySystem.Models
{
    public partial class Store
    {
        public Store()
        {
            StoreItems = new HashSet<StoreItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Image { get; set; }

        public virtual ICollection<StoreItem> StoreItems { get; set; }
    }
}
