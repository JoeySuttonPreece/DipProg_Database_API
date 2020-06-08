using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Location
    {
        public Location()
        {
            Inventory = new HashSet<Inventory>();
            Purchaseorder = new HashSet<Purchaseorder>();
        }

        public string Locationid { get; set; }
        public string Locname { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Purchaseorder> Purchaseorder { get; set; }
    }
}
