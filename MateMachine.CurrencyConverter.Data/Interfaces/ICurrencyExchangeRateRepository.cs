using MateMachine.CurrencyConverter.Data.Entities;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    internal interface ICurrencyExchangeRateRepository : IRepository<CurrenyExchangeRate> {
        CurrenyExchangeRate GetExchangeRate(Currency from, Currency to);
    }
}
