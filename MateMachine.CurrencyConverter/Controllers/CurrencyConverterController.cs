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
        }

        [HttpGet("Currencies")]
        public IActionResult GetAllCurrencies() {
            return Ok(_uow.CurrencyRepo.GetAll().Select(c => new CurrencyViewModel() {
                Name = c.Name,
                FullName = c.FullName
            }));
        }

        [HttpPost("Currencies")]
        public async Task<IActionResult> SaveCurrency([FromBody]CurrencyViewModel model) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var existingCurrency = _uow.CurrencyRepo.GetByName(model.Name);
            if (existingCurrency != null) {
                return BadRequest("Currency already exists");
            }

            var currency = new Currency() {
                Name = model.Name,
                FullName = model.FullName,
            };
            _uow.CurrencyRepo.Add(currency);
            try {
                await _uow.CompleteAsync();
                return Created("", currency);
            }
            catch (Exception ex) {
                // TODO: should return a proper error message
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
        public async Task<IActionResult> SaveOrUpdateExchangeRate([FromBody]ExchangeRateViewModel model) {
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
                await _currencyConverter.UpdateConfiguration(new List<(string, string, double)>() {
                    (model.FromCurrency, model.ToCurrency, model.ExchangeRate)
                });
                return Ok("Successful");
            }
            catch (Exception) {
                // TODO: Handle gracefully
                throw;
            }
        }
    }
}
