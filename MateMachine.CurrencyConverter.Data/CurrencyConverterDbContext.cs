using Microsoft.EntityFrameworkCore;
using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data {
    public class CurrencyConverterDbContext : DbContext {
        public CurrencyConverterDbContext(DbContextOptions<CurrencyConverterDbContext> options) : base(options) { }

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CurrenyExchangeRate> ExchangeRates { get; set; }
    }
}
