﻿using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using MateMachine.CurrencyConverter.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Business {
    public class CurrencyConverter : ICurrencyConverter {
        private IUnitOfWork _uow;

        public CurrencyConverter(IUnitOfWork uow) {
            _uow = uow;
        }

        public void ClearConfiguration() {
            throw new NotImplementedException();
        }

        public double? Convert(string fromCurrency, string toCurrency, double amount) {
            var source = _uow.CurrencyRepo.GetByName(fromCurrency);
            var destination = _uow.CurrencyRepo.GetByName(toCurrency);

            var directExchangeRate = _uow.ExchangeRateRepo.GetExchangeRate(source, destination);
            if (directExchangeRate == null) {
                var reverseExchangeRate = _uow.ExchangeRateRepo.GetExchangeRate(destination, source);
                if (reverseExchangeRate == null) {
                    CurrencyConversionGraph currencyConversionGraph = new CurrencyConversionGraph();
                    var allCurrencies = _uow.CurrencyRepo.GetAll();
                    var allExchangeRates = _uow.ExchangeRateRepo.GetAll();
                    currencyConversionGraph.AddNodes(allCurrencies);
                    currencyConversionGraph.AddEdges(allExchangeRates);
                    var shortestPath = currencyConversionGraph.GetShortestPath(source, destination);
                    if (shortestPath == null) {
                        return null;
                    }
                    else {
                        for (int i = 0; i < shortestPath.Count - 1; i++) {
                            var rate = allExchangeRates.SingleOrDefault(e => e.FromCurrency == shortestPath[i] && e.ToCurrency == shortestPath[i + 1]);
                            if (rate == null) {
                                rate = allExchangeRates.SingleOrDefault(e => e.FromCurrency == shortestPath[i + 1] && e.ToCurrency == shortestPath[i]);
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

        public async Task UpdateConfiguration(IEnumerable<(string FromCurrency, string ToCurrency, double ExchangeRate)> conversionRates) {
            foreach (var conversionRate in conversionRates) {
                var existingRate = _uow.ExchangeRateRepo.GetExchangeRate(conversionRate.FromCurrency, conversionRate.ToCurrency);
                if (existingRate == null) {
                    _uow.ExchangeRateRepo.Add(new CurrenyExchangeRate() {
                        FromCurrency = _uow.CurrencyRepo.GetByName(conversionRate.FromCurrency),
                        ToCurrency = _uow.CurrencyRepo.GetByName(conversionRate.ToCurrency),
                        ExchangeRate = conversionRate.ExchangeRate
                    });
                }
                else {
                    existingRate.ExchangeRate = conversionRate.ExchangeRate;
                }
            }
            await _uow.CompleteAsync();
        }
    }
}
