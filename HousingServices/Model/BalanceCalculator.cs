using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HousingServices.Model
{
    public class BalanceCalculator
    {
        public static BalanceCalculator LoadByAccountId(int AccountId)
        {
            BalanceCalculator calc = new BalanceCalculator();
            string _srcFinanceData = File.ReadAllText(@"Data\balance.json");
            calc.balance = JsonConvert.DeserializeObject<FinanceData>(_srcFinanceData).balance.Where(i => i.account_id == AccountId).ToList();
            if (!calc.balance.Any())
                throw new Exception($"Не найдена информация о начислениях по AccountId {AccountId}");
            string _srcPaymentData = File.ReadAllText(@"Data\payment.json");
            calc.payments = JsonConvert.DeserializeObject<List<Payment>>(_srcPaymentData).Where(i => i.account_id == AccountId).ToList();
            calc.CalcBalance = new List<CalcMonthBalance>();
            var SortBalance = calc.balance.Select(i => new
            {
                Date = new DateTime(i.period / 100, i.period % 100, 1),
                Calculation = i.calculation,
                InBalance = i.in_balance,
            }).OrderBy(i=> i.Date).ToList();

            //double FreeMoney = SortBalance.First().InBalance;
            double PrevCalc = SortBalance.First().InBalance;
            double PrevStartBalance = 0;
            foreach (var sb in SortBalance)
            {
                double paym = calc.payments.Where(p => p.date.Year == sb.Date.Year && p.date.Month == sb.Date.Month).Sum(p => p.sum);
                //if (InBalance == 0)
                //{
                //    InBalance += FreeMoney;
                //    FreeMoney = 0;
                //}
                /*
                double Balance = 0;
                if (TotalPayments == 0 && TotalCalculation == 0)
                    StartBalance = SBalance;
                else
                    StartBalance = TotalPayments - TotalCalculation;
                */

                /*
                double Calc = 
                TotalPayments += Payments;
                TotalCalculation += sb.Calculation;
                */
                //FreeMoney += InBalance - sb.Calculation;
                //double EndBalance = EndPrev + paym - sb.Calculation;
                calc.CalcBalance.Add(new CalcMonthBalance()
                {
                    Date = sb.Date,
                    Calculation = sb.Calculation, //sb.Calculation,
                    StartBalance = PrevCalc,//StartBalance,
                    EndBalance = sb.Calculation,//TotalPayments - TotalCalculation,
                    Payments = paym,//Payments
                    Usl = paym 
                });
                PrevCalc = sb.Calculation;
                //EndPrev = EndBalance;
                //StartBalance = EndBalance;
            }
            return calc;
        }
        private List<CalcMonthBalance> CalcBalance { get; set; }
        private List<Balance> balance { get; set; }
        private List<Payment> payments { get; set; }
        private double PaymentsByYear(int Year) =>
            payments.Where(p => p.date.Year == Year).Sum(p => p.sum);

        public void GroupByYear()
        {
            balance.GroupBy(gr => gr.period / 100).Select(i => new
            {
                Period = i.Key,
                BeginBalance = i.Sum(j=> j.calculation)
            });
        }
    }
}
