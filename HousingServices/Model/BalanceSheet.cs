using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace HousingServices.Model
{
    public class BalanceSheet
    {
        /// <summary>
        /// Наименование периода
        /// </summary>
        public string Period { get; set; }

        /// <summary>
        /// Входящее сальдо на начало периода
        /// </summary>
        public double InBalance { get; set; }

        /// <summary>
        /// Начислено за период
        /// </summary>
        public double Calculation { get; set; }

        /// <summary>
        /// Оплачено за период
        /// </summary>
        public double Payments { get; set; }

        /// <summary>
        /// Исходящий баланс на конец периода
        /// </summary>
        public double OutBalance { get; set; }

        [XmlIgnoreAttribute]
        public DateTime Date;
    }
}
