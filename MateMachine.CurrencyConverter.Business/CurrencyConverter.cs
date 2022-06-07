using MateMachine.CurrencyConverter.Data.Interfaces;
using MateMachine.CurrencyConverter.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    public class CurrencyConverter : ICurrencyConverter {
        private ICurrencyRepository _currencyRepository;

        public CurrencyConverter(ICurrencyRepository currencyRepository) {
            _currencyRepository = currencyRepository;
        }

        public void ClearConfiguration() {
            throw new NotImplementedException();
        }

        public double Convert(string fromCurrency, string toCurrency, double amount) {
            var source = _currencyRepository.GetByName(fromCurrency);
            var destination = _currencyRepository.GetByName(toCurrency);

            // We need the exchange rate repository here

            throw new NotImplementedException();
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates) {
            throw new NotImplementedException();
        }
    }
}
