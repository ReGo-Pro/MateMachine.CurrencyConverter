using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface ICurrencyExchangeRateRepository : IRepository<CurrenyExchangeRate> {
        CurrenyExchangeRate GetExchangeRate(Currency from, Currency to);
        CurrenyExchangeRate GetExchangeRate(string from, string to);
    }
}
