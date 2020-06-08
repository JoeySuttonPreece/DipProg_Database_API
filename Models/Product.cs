using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Product
    {
        public Product()
        {
            Inventory = new HashSet<Inventory>();
            Orderline = new HashSet<Orderline>();
            Purchaseorder = new HashSet<Purchaseorder>();
        }

        public int Productid { get; set; }
        public string Prodname { get; set; }
        public decimal? Buyprice { get; set; }
        public decimal? Sellprice { get; set; }

        public virtual ICollection<Inventory> Inventory { get; set; }
        public virtual ICollection<Orderline> Orderline { get; set; }
        public virtual ICollection<Purchaseorder> Purchaseorder { get; set; }
    }
}
