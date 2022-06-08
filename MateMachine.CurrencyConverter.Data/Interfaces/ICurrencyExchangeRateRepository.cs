using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface ICurrencyExchangeRateRepository : IRepository<CurrencyExchangeRate> {
        CurrencyExchangeRate GetExchangeRate(Currency from, Currency to);
        CurrencyExchangeRate GetExchangeRate(string from, string to);
    }
}
