using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingServices.Model
{
    public class Balance
    {
        public int account_id { get; set; }
        public int period { get; set; }
        public double in_balance { get; set; }
        public double calculation { get; set; }
    }
}
