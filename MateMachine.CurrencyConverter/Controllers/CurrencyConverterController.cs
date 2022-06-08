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
            return Ok(_uow.CurrencyRepo.GetAll());
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
    }
}
