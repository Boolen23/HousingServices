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
        public static double GetArrearsInPayment(int AccountId)
        {
            var data = LoadData(AccountId);

            double TotalPayments = data.Payments.Sum(i => i.sum);
            double TotalCalculation = data.Balance.Sum(i => i.calculation);
            return Math.Abs(TotalPayments - TotalCalculation);
        }

        public static (List<Payment> Payments, List<Balance> Balance) LoadData(int AccountId)
        {
            string _srcFinanceData = File.ReadAllText(@"Data\balance.json");
            var balance = JsonConvert.DeserializeObject<BalanceList>(_srcFinanceData).balance.Where(i => i.account_id == AccountId).ToList();
            if (!balance.Any())
                throw new Exception($"Не найдена информация о начислениях по AccountId {AccountId}");
            string _srcPaymentData = File.ReadAllText(@"Data\payment.json");
            var payments = JsonConvert.DeserializeObject<List<Payment>>(_srcPaymentData).Where(i => i.account_id == AccountId).ToList();
            return (payments, balance);
        }

        public static BalanceCalculator LoadByAccountId(int AccountId)
        {
            BalanceCalculator calc = new BalanceCalculator();
            var data = LoadData(AccountId);
            calc.CalculateBalance(data.Balance, data.Payments);
            return calc;
        }
        private List<BalanceSheet> CalcBalance { get; set; }
        
        private void CalculateBalance(List<Balance> balance, List<Payment> payments)
        {
            CalcBalance = new List<BalanceSheet>();
            var SortBalance = balance.Select(i => new
            {
                Date = new DateTime(i.period / 100, i.period % 100, 1),
                Calculation = i.calculation,
                InBalance = i.in_balance,
            }).OrderBy(i => i.Date).ToList();

            double StartBalance = SortBalance.First().InBalance;
            double FreeBalance = SortBalance.First().InBalance + 57.8;
            foreach (var sb in SortBalance)
            {
                double paym = payments.Where(p => sb.Date.AddDays(-10) <= p.date && sb.Date.AddDays(21) >= p.date).Sum(p => p.sum);

                var cmb = new BalanceSheet()
                {
                    Date = sb.Date,
                    Calculation = sb.Calculation,
                    InBalance = StartBalance,
                    Payments = paym,
                    Period = $"{sb.Date.Year}{(sb.Date.Month < 10 ? "0" : "")}{sb.Date.Month}"
                };
                double GetByFreeBalance = 0;
                if (FreeBalance > 0)
                {
                    GetByFreeBalance = FreeBalance >= StartBalance + sb.Calculation ? StartBalance + sb.Calculation : FreeBalance;
                    FreeBalance -= GetByFreeBalance;
                }
                StartBalance = Math.Round(StartBalance - paym - GetByFreeBalance, 2);
                if (StartBalance < 0)
                {
                    FreeBalance += Math.Abs(StartBalance);
                    StartBalance = 0;
                }
                StartBalance += sb.Calculation;
                cmb.OutBalance = StartBalance;
                CalcBalance.Add(cmb);
            }
        }
        public List<BalanceSheet> GetReport(int Period)
        {
            if(Period == 1)
            {
                return CalcBalance.OrderByDescending(i => i.Date).ToList();
            }
            if(Period == 2)
            {
                return CalcBalance.GroupBy(i => $"{i.Date.Year}_{((i.Date.Month - 1) / 3) + 1}").Select(i => new BalanceSheet()
                {
                    Period = i.Key,
                    Calculation = i.Sum(j => j.Calculation),
                    InBalance = i.Sum(j => j.InBalance),
                    Payments = i.Sum(j => j.Payments),
                    OutBalance = i.Sum(j => j.InBalance - j.Payments + j.Calculation)
                }).OrderByDescending(i=> i.Period).ToList();
            }
            if(Period == 3)
            {
                return CalcBalance.GroupBy(i => i.Date.Year).Select(i => new BalanceSheet()
                {
                    Period = i.Key.ToString(),
                    Calculation = i.Sum(j => j.Calculation),
                    InBalance = i.Sum(j => j.InBalance),
                    Payments = i.Sum(j => j.Payments),
                    OutBalance = i.Sum(j => j.InBalance - j.Payments + j.Calculation)
                }).OrderByDescending(i => i.Period).ToList();
            }
            return null;
        }
    }
}
