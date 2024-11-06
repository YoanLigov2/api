using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly CurrencyService _currencyService;

        public ValuesController(CurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("convert")]
        public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount)
        {
            try
            {
                var result = await _currencyService.GetCurrencyConversion(from, to, amount);
                return Ok(result);  // This should return a plain number
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

    }
}













