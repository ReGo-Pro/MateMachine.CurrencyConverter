using MateMachine.CurrencyConverter.Data.Entities;
using MateMachine.CurrencyConverter.Data.Interfaces;
using MateMachine.CurrencyConverter.Models;
using Microsoft.AspNetCore.Mvc;

namespace MateMachine.CurrencyConverter.Controllers {
    [Route("api/CurrencyConverter")]
    public class CurrencyConverterController : Controller {
        private IUnitOfWork _uow;
        public CurrencyConverterController(IUnitOfWork uow) {
            _uow = uow;
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
    }
}
