using MateMachine.CurrencyConverter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    public interface ICurrencyConverter {
        public bool IsInitialzied { get; }
        void Initialize(IEnumerable<Currency> allCurrencies, IEnumerable<CurrenyExchangeRate> allExchangeRates);
        /// <summary> 
        /// Clears any prior configuration. 
        /// </summary> 
        void ClearConfiguration();

        /// <summary> 
        /// Updates the configuration. Rates are inserted or replaced internally. 
        /// </summary> 
        void UpdateConfiguration(IEnumerable<(Currency FromCurrency, Currency ToCurrency, double ExchangeRate)> conversionRates);

        /// <summary> 
        /// Converts the specified amount to the desired currency. 
        /// </summary> 
        double? Convert(Currency fromCurrency, Currency toCurrency, double amount);
    }
}