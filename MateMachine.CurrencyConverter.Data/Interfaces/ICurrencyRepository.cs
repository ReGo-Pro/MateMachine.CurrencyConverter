using MateMachine.CurrencyConverter.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MateMachine.CurrencyConverter.Data.Interfaces {
    public interface ICurrencyRepository : IRepository<Currency> {
        public Currency GetByName(string name);
    }
}
