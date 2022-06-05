using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace MateMachine.CurrencyConverter.Data {
    internal class DbContextFactory : IDesignTimeDbContextFactory<CurrencyConverterDbContext> {
        public CurrencyConverterDbContext CreateDbContext(string[] args) {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var optionsBuilder = new DbContextOptionsBuilder<CurrencyConverterDbContext>();
            optionsBuilder.UseSqlServer(/*args[0]*/ config.GetConnectionString("DbConnex"));
            return new CurrencyConverterDbContext(optionsBuilder.Options);
        }
    }
}
