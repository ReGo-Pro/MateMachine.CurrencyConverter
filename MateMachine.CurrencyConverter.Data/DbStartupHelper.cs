using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MateMachine.CurrencyConverter.Data {
    public static class DbStartupHelper {
        public static void AddDbContextTransient(IServiceCollection services) {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            services.AddDbContext<CurrencyConverterDbContext>(options => options.UseSqlServer(config.GetConnectionString("DbConnex")));
        }
    }
}
