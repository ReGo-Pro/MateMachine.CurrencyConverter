using MateMachine.CurrencyConverter.Data.Interfaces;
using MateMachine.CurrencyConverter.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data {
    public class UnitOfWork : IUnitOfWork {
        private CurrencyConverterDbContext _dbContext;
        public UnitOfWork(CurrencyConverterDbContext dbContext) {
            _dbContext = dbContext;
            CurrencyRepo = new CurrencyRepository(dbContext);
            ExchangeRateRepo = new CurrencyExchangeRateRepository(dbContext);
        }

        public ICurrencyRepository CurrencyRepo { get; private set;}
        public ICurrencyExchangeRateRepository ExchangeRateRepo { get; private set; }

        public async Task CompleteAsync() {
            await _dbContext.SaveChangesAsync();
        }
    }
}
