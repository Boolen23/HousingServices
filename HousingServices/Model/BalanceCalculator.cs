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
            return calc;
        }
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
