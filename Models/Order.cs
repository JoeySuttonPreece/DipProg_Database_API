using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Order
    {
        public Order()
        {
            Orderline = new HashSet<Orderline>();
        }

        public int Orderid { get; set; }
        public string Shippingaddress { get; set; }
        public DateTime Datetimecreated { get; set; }
        public DateTime? Datetimedispatched { get; set; }
        public decimal Total { get; set; }
        public int Userid { get; set; }

        public virtual Authorisedperson User { get; set; }
        public virtual ICollection<Orderline> Orderline { get; set; }
    }
}
