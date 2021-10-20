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

            double Saldo = SortBalance.First().InBalance; 
            double FreeBalance = SortBalance.First().InBalance + 57.8; 
            foreach (var sb in SortBalance)
            {
                double paym = calc.payments.Where(p => sb.Date.AddDays(-10) <= p.date && sb.Date.AddDays(21) >= p.date).Sum(p => p.sum);

                var cmb = new CalcMonthBalance()
                {
                    Date = sb.Date,
                    Calculation = sb.Calculation, 
                    StartBalance = Saldo,
                    Payments = paym,
                };
                double GetByFreeBalance = 0;
                if (FreeBalance > 0)
                {
                    GetByFreeBalance = FreeBalance >= Saldo + sb.Calculation ? Saldo + sb.Calculation : FreeBalance;
                    FreeBalance -= GetByFreeBalance;
                }
                Saldo = Math.Round(Saldo - paym - GetByFreeBalance, 2);
                if(Saldo < 0)
                {
                    FreeBalance += Math.Abs(Saldo);
                    Saldo = 0;
                }
                Saldo += sb.Calculation;
                cmb.EndBalance = Saldo;
                calc.CalcBalance.Add(cmb);
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
