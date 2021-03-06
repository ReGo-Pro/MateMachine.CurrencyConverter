using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using System.Collections.Concurrent;

namespace MateMachine.CurrencyConverter.Business {
    public class CurrencyConverter : ICurrencyConverter {
        private ConcurrentBag<Currency> _allCurrencies;
        private ConcurrentBag<CurrencyExchangeRate> _allExchangeRates;
        private ConcurrentDictionary<(Currency, Currency), List<Currency>> _routeCache;

        public bool IsInitialzied { get; private set; }

        public CurrencyConverter() {
            _allCurrencies = new ConcurrentBag<Currency>();
            _allExchangeRates = new ConcurrentBag<CurrencyExchangeRate>();
            _routeCache = new ConcurrentDictionary<(Currency,Currency), List<Currency>>();
        }

        public void ClearConfiguration() {
            _allCurrencies.Clear();
            _allExchangeRates.Clear();
            _routeCache.Clear();
        }

        // This method is still unsafe
        private double? Convert(Currency fromCurrency, Currency toCurrency, double amount) {
            var directExchangeRate = _allExchangeRates.SingleOrDefault(c => c.FromCurrencyId == fromCurrency.Id && c.ToCurrencyId == toCurrency.Id);
            if (directExchangeRate == null) {
                var reverseExchangeRate = _allExchangeRates.SingleOrDefault(c => c.FromCurrencyId == toCurrency.Id && c.ToCurrencyId == fromCurrency.Id);
                if (reverseExchangeRate == null) {
                    List<Currency> shortestPath = null;
                    if (_routeCache.ContainsKey((fromCurrency, toCurrency))) {
                        shortestPath = _routeCache[(fromCurrency, toCurrency)];
                    }
                    else {
                        CurrencyConversionGraph currencyConversionGraph = new CurrencyConversionGraph();
                        currencyConversionGraph.AddNodes(_allCurrencies);
                        currencyConversionGraph.AddEdges(_allExchangeRates);
                        shortestPath = currencyConversionGraph.GetShortestPath(fromCurrency, toCurrency);
                        _routeCache[(fromCurrency, toCurrency)] = shortestPath;
                    }
                    if (shortestPath == null) {
                        return null;
                    }
                    else {
                        for (int i = 0; i < shortestPath.Count - 1; i++) {
                            var rate = _allExchangeRates.SingleOrDefault(e => e.FromCurrency.Name == shortestPath[i].Name && e.ToCurrency.Name == shortestPath[i + 1].Name);
                            if (rate == null) {
                                rate = _allExchangeRates.SingleOrDefault(e => e.FromCurrency.Name == shortestPath[i + 1].Name && e.ToCurrency.Name == shortestPath[i].Name);
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

        public async Task<double?> ConvertAsync(Currency fromCurrency, Currency toCurrency, double amount) {
            return await Task.Run(() => Convert(fromCurrency, toCurrency, amount));
        }

        public void UpdateConfiguration(IEnumerable<(Currency FromCurrency, Currency ToCurrency, double ExchangeRate)> conversionRates) {
            IsInitialzied = true;

            foreach (var conversionRate in conversionRates) {
                if (!_allCurrencies.Contains(conversionRate.FromCurrency)) {
                    _allCurrencies.Add(conversionRate.FromCurrency);
                    _routeCache.Clear();
                    // Adding a new node renders cache invalid
                }
                if (!_allCurrencies.Contains((Currency)conversionRate.ToCurrency)) {
                    _allCurrencies.Add(conversionRate.ToCurrency);
                    _routeCache.Clear();
                    // Adding a new node renders cache invalid
                }
                var existingRate = _allExchangeRates.FirstOrDefault(er => er.FromCurrencyId == conversionRate.FromCurrency.Id && er.ToCurrencyId == conversionRate.ToCurrency.Id);
                if (existingRate == null) {
                    _allExchangeRates.Add(new CurrencyExchangeRate() {
                        FromCurrency = conversionRate.FromCurrency,
                        FromCurrencyId = conversionRate.FromCurrency.Id,
                        ToCurrency = conversionRate.ToCurrency,
                        ToCurrencyId = conversionRate.ToCurrency.Id,
                        ExchangeRate = conversionRate.ExchangeRate
                    });
                    _routeCache.Clear();    // Adding new edge renders this cache invalid
                }
                else {
                    // I do not cache exchange rates themselves because they tend to change frequently and updating the cache every time creates extra overhead.
                    // But it is less likely to add new node (currency) or edge (exchange rate) to our class
                    existingRate.ExchangeRate = conversionRate.ExchangeRate;
                }
            }
        }
    }
}
