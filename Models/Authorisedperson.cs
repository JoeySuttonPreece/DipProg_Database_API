using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Authorisedperson
    {
        public Authorisedperson()
        {
            Order = new HashSet<Order>();
        }

        public int Userid { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Accountid { get; set; }

        public virtual Clientaccount Account { get; set; }
        public virtual ICollection<Order> Order { get; set; }
    }
}
