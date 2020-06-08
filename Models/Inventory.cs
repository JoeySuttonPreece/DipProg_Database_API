using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Inventory
    {
        public int Productid { get; set; }
        public string Locationid { get; set; }
        public int Numinstock { get; set; }

        public virtual Location Location { get; set; }
        public virtual Product Product { get; set; }
    }
}
