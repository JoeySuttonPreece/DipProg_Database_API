using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Purchaseorder
    {
        public int Productid { get; set; }
        public string Locationid { get; set; }
        public DateTime Datetimecreated { get; set; }
        public int? Quantity { get; set; }
        public decimal? Total { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}
