using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using System.Collections.Concurrent;

namespace MateMachine.CurrencyConverter.Business {
    public class CurrencyConverter : ICurrencyConverter {
        private ConcurrentBag<Currency> _allCurrencies;
        private ConcurrentBag<CurrencyExchangeRate> _allExchangeRates;
        public bool IsInitialzied { get; private set; }

        public CurrencyConverter() {
            _allCurrencies = new ConcurrentBag<Currency>();
            _allExchangeRates = new ConcurrentBag<CurrencyExchangeRate>();
        }

        public void ClearConfiguration() {
            _allCurrencies.Clear();
            _allExchangeRates.Clear();
        }

        // This method is still unsafe
        public double? Convert(Currency fromCurrency, Currency toCurrency, double amount) {
            var directExchangeRate = _allExchangeRates.SingleOrDefault(c => c.FromCurrencyId == fromCurrency.Id && c.ToCurrencyId == toCurrency.Id);
            if (directExchangeRate == null) {
                var reverseExchangeRate = _allExchangeRates.SingleOrDefault(c => c.FromCurrencyId == toCurrency.Id && c.ToCurrencyId == fromCurrency.Id);
                if (reverseExchangeRate == null) {
                    CurrencyConversionGraph currencyConversionGraph = new CurrencyConversionGraph();
                    currencyConversionGraph.AddNodes(_allCurrencies);
                    currencyConversionGraph.AddEdges(_allExchangeRates);
                    var shortestPath = currencyConversionGraph.GetShortestPath(fromCurrency, toCurrency);
                    if (shortestPath == null) {
                        return null;
                    }
                    else {
                        for (int i = 0; i < shortestPath.Count - 1; i++) {
                            var rate = _allExchangeRates.SingleOrDefault(e => e.FromCurrency == shortestPath[i] && e.ToCurrency == shortestPath[i + 1]);
                            if (rate == null) {
                                rate = _allExchangeRates.SingleOrDefault(e => e.FromCurrency == shortestPath[i + 1] && e.ToCurrency == shortestPath[i]);
                                amount *= (1 / rate.ExchangeRate);
                            }
                            else {
                                amount *= rate.ExchangeRate;
                            }
                        }

                        return amount;
                    }
                }
                return amount / reverseExchangeRate.ExchangeRate;
            }
            return amount * directExchangeRate.ExchangeRate;
        }

        public void UpdateConfiguration(IEnumerable<(Currency FromCurrency, Currency ToCurrency, double ExchangeRate)> conversionRates) {
            IsInitialzied = true;
            foreach (var conversionRate in conversionRates) {
                if (!_allCurrencies.Contains(conversionRate.FromCurrency)) {
                    _allCurrencies.Add(conversionRate.FromCurrency);
                }
                if (!_allCurrencies.Contains((Currency)conversionRate.ToCurrency)) {
                    _allCurrencies.Add(conversionRate.ToCurrency);
                }
                var existingRate = _allExchangeRates.FirstOrDefault(er => er.FromCurrencyId == conversionRate.FromCurrency.Id && er.ToCurrencyId == conversionRate.ToCurrency.Id);
                if (existingRate == null) {
                    _allExchangeRates.Add(new CurrencyExchangeRate() {
                        FromCurrency = conversionRate.FromCurrency,
                        ToCurrency = conversionRate.ToCurrency,
                        ExchangeRate = conversionRate.ExchangeRate
                    });
                }
                else {
                    existingRate.ExchangeRate = conversionRate.ExchangeRate;
                }
            }
        }
    }
}
