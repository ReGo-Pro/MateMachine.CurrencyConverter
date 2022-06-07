using MateMachine.CurrencyConverter.Data.Entities;
using Microsoft.EntityFrameworkCore;
using MateMachine.CurrencyConverter.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Repositories {
    public class CurrencyRepository : GenericRepository<Currency>, ICurrencyRepository {
        public CurrencyRepository(CurrencyConverterDbContext dbContext) : base(dbContext) {
        }

        public Currency GetByName(string name) {
            name = name.ToUpper();
            return DbContext.Currencies.Single(c => c.Name == name);
        }

        private CurrencyConverterDbContext DbContext {
            get => dbContext as CurrencyConverterDbContext;
        }
    }
}
