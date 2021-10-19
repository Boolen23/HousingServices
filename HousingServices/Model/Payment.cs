using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingServices.Model
{
    public class Payment
    {
        public int account_id { get; set; }
        public DateTime date { get; set; }
        public double sum { get; set; }
        public string payment_guid { get; set; }
    }
}
