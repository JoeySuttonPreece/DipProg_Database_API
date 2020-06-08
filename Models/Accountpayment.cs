using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Accountpayment
    {
        public int Accountid { get; set; }
        public DateTime Datetimereceived { get; set; }
        public decimal Amount { get; set; }

        public virtual Clientaccount Account { get; set; }
    }
}
