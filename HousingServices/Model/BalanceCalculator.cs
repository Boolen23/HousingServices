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

            var OutlaySum = calc.balance.Sum(i => i.calculation);
            var payments = calc.payments.Sum(i => i.sum);
            return calc;
        }
        private List<Balance> balance { get; set; }
        private List<Payment> payments { get; set; }
    }
}
