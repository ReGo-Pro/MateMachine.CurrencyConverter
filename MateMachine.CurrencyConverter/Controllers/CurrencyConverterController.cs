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
                return Created("", newCurrencies);
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
        public IActionResult SaveOrUpdateExchangeRate([FromBody]ExchangeRateViewModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            // This lookup is redundent, take care of this
            var fromCurrency = _uow.CurrencyRepo.GetByName(model.FromCurrency);
            if (fromCurrency == null) {
                return BadRequest($"Invalid currency {model.FromCurrency}");
            }
            var toCurrency = _uow.CurrencyRepo.GetByName(model.ToCurrency);
            if (toCurrency == null) {
                return BadRequest($"Invalid currency {model.ToCurrency}");
            }

            try {
                _currencyConverter.UpdateConfiguration(new List<(Currency, Currency, double)>() { (fromCurrency, toCurrency, model.ExchangeRate) });
                return Ok("Successful");
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
