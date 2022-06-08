using MateMachine.CurrencyConverter.Business;
using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using MateMachine.CurrencyConverter.Models;
using Microsoft.AspNetCore.Mvc;

namespace MateMachine.CurrencyConverter.Controllers {
    [Route("api/CurrencyConverter")]
    public class CurrencyConverterController : Controller {
        private IUnitOfWork _uow;
        private ICurrencyConverter _currencyConverter;

        public CurrencyConverterController(IUnitOfWork uow, ICurrencyConverter currencyConverter) {
            _uow = uow;
            _currencyConverter = currencyConverter;
            if (!_currencyConverter.IsInitialzied) {
                _currencyConverter.UpdateConfiguration(_uow.ExchangeRateRepo
                                                           .GetAll()
                                                           .Select(er => (er.FromCurrency, er.ToCurrency, er.ExchangeRate)));
            }
        }

        [HttpGet("Currencies")]
        public IActionResult GetAllCurrencies() {
            return Ok(_uow.CurrencyRepo.GetAll().Select(c => new CurrencyViewModel() {
                Name = c.Name,
                FullName = c.FullName
            }));
        }

        [HttpPost("Currencies")]
        public async Task<IActionResult> SaveCurrencies([FromBody] IEnumerable<CurrencyViewModel> model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var newCurrencies = new List<Currency>();
            foreach (var currencyModel in model) {
                var existingCurrency = _uow.CurrencyRepo.GetByName(currencyModel.Name);
                if (existingCurrency != null) {
                    return BadRequest("Currency already exists");
                }
                newCurrencies.Add(new Currency() {
                    Name = currencyModel.Name,
                    FullName = currencyModel.FullName,
                });
            }
            try {
                _uow.CurrencyRepo.AddRange(newCurrencies);
                await _uow.CompleteAsync();
                return Created("", newCurrencies.Select(c => new CurrencyViewModel() {
                    FullName = c.FullName,
                    Name = c.Name
                }));
            }
            catch (Exception) {
                // TODO: Handle gracefully
                throw;
            }
        }

        [HttpGet("ExchangeRates")]
        public IActionResult GetExchangeRates() {
            return Ok(_uow.ExchangeRateRepo.GetAll().Select(er => new ExchangeRateViewModel() {
                FromCurrency = er.FromCurrency.Name,
                ToCurrency = er.ToCurrency.Name,
                ExchangeRate = er.ExchangeRate
            }));
        }

        [HttpPost("ExchangeRates")]
        public async Task<IActionResult> SaveExchangeRate([FromBody]IEnumerable<ExchangeRateViewModel> model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var newExchangeRates = new List<CurrencyExchangeRate>();
            foreach (var erModel in model) {
                var fromCurrency = _uow.CurrencyRepo.GetByName(erModel.FromCurrency);
                if (fromCurrency == null) {
                    return BadRequest($"Invalid currency {erModel.FromCurrency}");
                }
                var toCurrency = _uow.CurrencyRepo.GetByName(erModel.ToCurrency);
                if (toCurrency == null) {
                    return BadRequest($"Invalid currency {erModel.ToCurrency}");
                }
                newExchangeRates.Add(new CurrencyExchangeRate() {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    ExchangeRate = erModel.ExchangeRate
                });
            }

            try {
                _uow.ExchangeRateRepo.AddRange(newExchangeRates);
                await _uow.CompleteAsync();
                _currencyConverter.UpdateConfiguration(newExchangeRates.Select(c => (c.FromCurrency, c.ToCurrency, c.ExchangeRate)));
                return Created("", newExchangeRates.Select(er => new ExchangeRateViewModel() {
                    FromCurrency = er.FromCurrency.Name,
                    ToCurrency= er.ToCurrency.Name,
                    ExchangeRate = er.ExchangeRate
                }));
            }
            catch (Exception) {
                // TODO: Handle gracefully
                throw;
            }
        }

        [HttpGet("Convert/{FromCurrency}/{ToCurrency}/{Amount}")]
        public IActionResult Convert(string FromCurrency, string ToCurrency, double Amount) {
            var fromCurrency = _uow.CurrencyRepo.GetByName(FromCurrency);
            if (fromCurrency == null) {
                return BadRequest($"Invalid currency {FromCurrency}");
            }
            var toCurrency = _uow.CurrencyRepo.GetByName(ToCurrency);
            if (toCurrency == null) {
                return BadRequest($"Invalid currency {ToCurrency}");
            }

            var convertedAmount = _currencyConverter.Convert(fromCurrency, toCurrency, Amount);
            if (convertedAmount == null) {
                return NotFound($"No path was found from {FromCurrency} to {ToCurrency}");
            }
            return Ok();
        }
    }
}
