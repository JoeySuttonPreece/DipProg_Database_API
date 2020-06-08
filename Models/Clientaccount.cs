using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Clientaccount
    {
        public Clientaccount()
        {
            Accountpayment = new HashSet<Accountpayment>();
            Authorisedperson = new HashSet<Authorisedperson>();
        }

        public int Accountid { get; set; }
        public string Acctname { get; set; }
        public decimal Balance { get; set; }
        public decimal Creditlimit { get; set; }

        public virtual ICollection<Accountpayment> Accountpayment { get; set; }
        public virtual ICollection<Authorisedperson> Authorisedperson { get; set; }
    }
}
