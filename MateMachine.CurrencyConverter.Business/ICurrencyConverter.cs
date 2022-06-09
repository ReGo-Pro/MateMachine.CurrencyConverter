using MateMachine.CurrencyConverter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    public interface ICurrencyConverter {
        /// <summary>
        /// Determines whether class is already initialzed
        /// </summary>
        public bool IsInitialzied { get; }

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
        Task<double?> ConvertAsync(Currency fromCurrency, Currency toCurrency, double amount);
    }
}