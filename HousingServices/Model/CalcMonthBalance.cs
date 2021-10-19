using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HousingServices.Model
{
    public class CalcMonthBalance
    {
        public DateTime Date { get; set; }
        public double Calculation { get; set; }
        public double EndBalance { get; set; }
        public double StartBalance { get; set; }
        public double Payments { get; set; }
        public double Usl { get; set; }
    }
}
