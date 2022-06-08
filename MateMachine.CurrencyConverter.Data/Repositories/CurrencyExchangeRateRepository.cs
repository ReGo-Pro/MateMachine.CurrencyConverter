using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Repositories {
    public class CurrencyExchangeRateRepository : GenericRepository<CurrenyExchangeRate>, ICurrencyExchangeRateRepository {
        public CurrencyExchangeRateRepository(CurrencyConverterDbContext dbContext) : base(dbContext) {
        }

        public CurrenyExchangeRate GetExchangeRate(Currency from, Currency to) {
            return DbContext.ExchangeRates.FirstOrDefault(e => e.FromCurrency == from && e.ToCurrency == to);
        }

        public CurrenyExchangeRate GetExchangeRate(string from, string to) {
            from = from.ToUpper();
            to = to.ToUpper();
            return DbContext.ExchangeRates.FirstOrDefault(e => e.FromCurrency.Name == from && e.ToCurrency.Name == to);
        }

        private CurrencyConverterDbContext DbContext {
            get => dbContext as CurrencyConverterDbContext;
        }
    }
}
