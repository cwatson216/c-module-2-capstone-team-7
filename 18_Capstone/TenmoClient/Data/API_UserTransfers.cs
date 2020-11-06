using System;
using System.Collections.Generic;
using System.Text;

namespace TenmoClient.Data
{
    public class API_UserTransfers
    {
        public int TransferId { get; set; }
        public int Type { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public decimal Amount { get; set; }
    }
}
