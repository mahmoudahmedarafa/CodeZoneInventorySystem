using System;
using System.Collections.Generic;

namespace CodeZoneInventorySystem.Models
{
    public partial class StoreItem
    {
        public int StoreId { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public DateTime DateReceived { get; set; }

        public virtual Item Item { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
    }
}
