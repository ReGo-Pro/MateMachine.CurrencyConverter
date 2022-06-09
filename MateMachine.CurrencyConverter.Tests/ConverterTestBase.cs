using MateMachine.CurrencyConverter.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MateMachine.CurrencyConverter.Business;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace MateMachine.CurrencyConverter.Tests {
    public abstract class ConverterTestBase {
        protected List<Currency> Currencies;
        protected List<CurrencyExchangeRate> ExchangeRates;
        protected ICurrencyConverter CurrencyConverter;
        protected double InitialAmount = 10;
        protected double? Result = null;

        public ConverterTestBase() {
            Currencies = new List<Currency>() {
                new Currency() {
                    Id = 1,
                    Name = "USD",
                    FullName = "United States dollar"
                },
                new Currency() {
                    Id = 2,
                    Name = "CAD",
                    FullName = "Canadian dollar"
                },
                new Currency() {
                    Id = 3,
                    Name = "EUR",
                    FullName = "Euro"
                },
                new Currency() {
                    Id = 4,
                    Name = "GBP",
                    FullName = "Great british pound"
                },
                new Currency() {
                    Id = 5,
                    Name = "SEK",
                    FullName = "Swedish krona"
                },
                new Currency() {
                    Id = 6,
                    Name = "JPY",
                    FullName = "Japanese yen"
                },
                new Currency() {
                    Id = 7,
                    Name = "CZK",
                    FullName = "Czech koruna"
                },
                new Currency() {
                    Id = 8,
                    Name = "EGP",
                    FullName = "Egyptian pound"
                },
                new Currency() {
                    Id = 9,
                    Name = "INR",
                    FullName = "Indian rupee"
                },
                new Currency() {
                    Id = 10,
                    Name = "AUD",
                    FullName = "Australian dollar"
                },
            };
            ExchangeRates = new List<CurrencyExchangeRate>() {
                new CurrencyExchangeRate() {
                    FromCurrency = Currencies.Single(c => c.Name == "USD"),
                    ToCurrency = Currencies.Single(c => c.Name == "CAD"),
                    ExchangeRate = 1.34
                },
                new CurrencyExchangeRate() {
                    FromCurrency = Currencies.Single(c => c.Name == "CAD"),
                    ToCurrency = Currencies.Single(c => c.Name == "GBP"),
                    ExchangeRate = 0.58
                },
                new CurrencyExchangeRate() {
                    FromCurrency = Currencies.Single(c => c.Name == "USD"),
                    ToCurrency = Currencies.Single(c => c.Name == "EUR"),
                    ExchangeRate = 0.86
                },
            };
            CurrencyConverter = new Business.CurrencyConverter();
            if (!CurrencyConverter.IsInitialzied) {
                CurrencyConverter.UpdateConfiguration(ExchangeRates.Select(er => (er.FromCurrency, er.ToCurrency, er.ExchangeRate)));
            }
        }

        public virtual void Arrange() { }
        public virtual async Task Act(string from, string to) { 
            Result = await CurrencyConverter.ConvertAsync(Currencies.Single(c => c.Name == from.ToUpper()), 
                Currencies.Single(c => c.Name == to.ToUpper()), 
                InitialAmount);
            if (Result != null) {
                Result = Math.Round(Result.Value, 5);
            }
        }
    }
}