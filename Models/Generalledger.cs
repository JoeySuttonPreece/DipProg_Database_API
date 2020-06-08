using System;
using System.Collections.Generic;

namespace Dip_DatabaseAPI.Models
{
    public partial class Generalledger
    {
        public int Itemid { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
    }
}
