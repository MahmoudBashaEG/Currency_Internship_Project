using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proj.Core;
using Proj.Core.Domains;
using Proj.Core.DTOs;
using Proj.Core.Exceptions;
using Proj.Core.Services;

namespace Proj.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrncyServices _currncyServices;
        public CurrencyController(ICurrncyServices currncyServices)
        {
            _currncyServices = currncyServices;
        }


        [HttpGet]
        public IActionResult GetAllCurrencies()
        {
            try
            {
                var currencies = _currncyServices.GetAllCurrencies();
                return Ok(currencies);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrency([FromBody] NewCurrencyDTO currency)
        {
            if (currency == null)
                return BadRequest("You Have To Fill The Body");

            try
            {
                 await _currncyServices.AddCurrency(currency);
                return Ok("Added Successfully");
            }
            catch (RepeatedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCurrency([FromBody] FoundedCurrencyDTO currency)
        {
            if (currency == null)
               return BadRequest("You Have To Fill The Body");
            try
            {
                await _currncyServices.UpdateCurrency(currency);
                return Ok("Updated Successfully");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (RepeatedException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCurrency([FromBody] FoundedCurrencyDTO currency)
        {
            if (currency == null)
                return BadRequest("You Have To Fill The Body");
            try
            {
                await _currncyServices.DeleteCurrency(currency);
                return Ok("Deleted Successfully");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrencyByName([FromQuery] string name)
        {
            if (name == null || name == "")
                return BadRequest("You Have to Enter The Sign Of The Currency");

            try
            {
                var cur = await this._currncyServices.GetCurrencyByName(name);
                return Ok(cur);
            }
            catch (NotFoundException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
        //GetHighestOrLowestNCurrenciesBetweentwoDates

        [HttpGet("{count}")]
        public IActionResult GetHighestNCurrencies([FromRoute] int count)
        {
            if (count <= 0)
                return BadRequest("Count Must Be Bigger Than 0");
            try
            {
                var res = this._currncyServices.GetHighestOrLowestNCurrencies(count,OrderType.Descending);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{count}")]
        public IActionResult GetLowestNCurrencies([FromRoute] int count)
        {
            if (count <= 0)
                return BadRequest("Count Must Be Bigger Than 0");
            try
            {
                var res = this._currncyServices.GetHighestOrLowestNCurrencies(count, OrderType.Ascending);
                return Ok(res);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult GetHighestImprovingNCurrenciesBetweenTwoDates([FromBody] CurrenciesBetweenTwoDates query)
        {
            if (query == null)
                return NotFound("You Have To Enter The Body");
            if (query.Count <= 0)
                return BadRequest("Count Must Be Bigger Than 0");
            try
            {
                var res = this._currncyServices.GetHighestOrLowestImprovedNCurrenciesBetweentwoDates(query, OrderType.Descending);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult GetLowestImprovingNCurrenciesBetweenTwoDates([FromBody] CurrenciesBetweenTwoDates query)
        {
            if (query == null)
                return NotFound("You Have To Enter The Body");
            if (query.Count <= 0)
                return BadRequest("Count Must Be Bigger Than 0");
            try
            {
                var res = this._currncyServices.GetHighestOrLowestImprovedNCurrenciesBetweentwoDates(query, OrderType.Ascending);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddCurrencyExchange([FromBody] ExchangeHistoryDTO currencyExchange)
        {
            if (currencyExchange == null)
                return BadRequest("You Have To Fill The Body");

            try
            {
                await this._currncyServices.AddCurrencyExchange(currencyExchange);
                return Ok("Added Successfully");
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Convert([FromBody] AmountConvert amount)
        {
            if (amount == null)
                return BadRequest("You Have To Enter The Body");

            if (amount.Amount < 0)
                return BadRequest("You Have To Amout More Than Or Equal To Zero");


            amount.ToSign = amount.ToSign.ToUpper();
            amount.FromSign = amount.FromSign.ToUpper();

            try
            {
                float res = await this._currncyServices.ConvertCurrency(amount);
                return Ok(res);
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
