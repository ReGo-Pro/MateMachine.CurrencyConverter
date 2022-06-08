using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    public interface ICurrencyConverter {
        /// <summary> 
        /// Clears any prior configuration. 
        /// </summary> 
        void ClearConfiguration();

        /// <summary> 
        /// Updates the configuration. Rates are inserted or replaced internally. 
        /// </summary> 
        Task UpdateConfiguration(IEnumerable<(string FromCurrency, string ToCurrency, double ExchangeRate)> conversionRates);

        /// <summary> 
        /// Converts the specified amount to the desired currency. 
        /// </summary> 
        double? Convert(string fromCurrency, string toCurrency, double amount);
    }
}