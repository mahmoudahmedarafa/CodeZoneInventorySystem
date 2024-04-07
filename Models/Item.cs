using System;
using System.Collections.Generic;

namespace CodeZoneInventorySystem.Models
{
    public partial class Item
    {
        public Item()
        {
            StoreItems = new HashSet<StoreItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Image { get; set; }

        public virtual ICollection<StoreItem> StoreItems { get; set; }
    }
}
