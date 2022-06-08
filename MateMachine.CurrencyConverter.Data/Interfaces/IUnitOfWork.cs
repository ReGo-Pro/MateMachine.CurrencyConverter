using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface IUnitOfWork {
        ICurrencyRepository CurrencyRepo { get; }
        ICurrencyExchangeRateRepository ExchangeRateRepo { get; }
        Task CompleteAsync();
    }
}
