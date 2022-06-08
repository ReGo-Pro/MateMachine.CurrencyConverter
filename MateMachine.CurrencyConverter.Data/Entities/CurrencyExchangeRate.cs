using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Entities {
    public class CurrencyExchangeRate {
        public int Id { get; set; }

        public int FromCurrencyId { get; set; }
        public Currency FromCurrency { get; set; }

        public int ToCurrencyId { get; set; }
        public Currency ToCurrency { get; set; }

        public double ExchangeRate { get; set; }
    }
}
