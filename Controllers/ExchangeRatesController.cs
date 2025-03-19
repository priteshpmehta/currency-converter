using currency_converter.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace currency_converter.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/ExchangeRates")]
public class ExchangeRatesController : ControllerBase
{
    private readonly ILogger<ExchangeRatesController> _logger;
    private readonly ICurrencyProviderFactory _currencyProviderFactory;

    public ExchangeRatesController(ILogger<ExchangeRatesController> logger, ICurrencyProviderFactory currencyProviderFactory)
    {
        _logger = logger;
        _currencyProviderFactory = currencyProviderFactory;
    }

    [HttpGet("latest")]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> GetLatestRates([FromQuery] string baseCurrency = "EUR", [FromQuery] string provider = "Frankfurter")
    {
        try
        {
            _logger.LogInformation($"Request received {HttpContext.Request.Path}");

            var providerInstance = _currencyProviderFactory.GetProvider(provider);
            var response = await providerInstance.GetLatestRates(baseCurrency);

            if (response == null)
                return BadRequest("Currency not supported");

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLatestRates");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("convert")]
    [Authorize(Roles = "User, Admin")]
    public async Task<IActionResult> ConvertCurrency([FromQuery] string from, [FromQuery] string to, [FromQuery] decimal amount, [FromQuery] string provider = "Frankfurter")
    {
        try
        {
            _logger.LogInformation($"Request received {HttpContext.Request.Path}");

            var providerInstance = _currencyProviderFactory.GetProvider(provider);
            var result = await providerInstance.ConvertCurrency(from, to, amount);
            
            if (result == null) 
                return BadRequest("Currency not supported.");
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ConvertCurrency");
            return StatusCode(500, "Internal Server Error");
        }
    }

    [HttpGet("historical")]
    public async Task<IActionResult> GetHistoricalRates([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] string baseCurrency = "EUR", 
        [FromQuery] string provider = "Frankfurter", [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            if (page < 1 || pageSize < 1 || pageSize > 100)
                return BadRequest("Page must be >= 1 and PageSize must be between 1-100.");

            if (endDate < startDate)
                return BadRequest("End date must be after start date.");

            var providerInstance = _currencyProviderFactory.GetProvider(provider);
            var response = await providerInstance.GetHistoricalExchangeRates(startDate, endDate, baseCurrency, page, pageSize);

            if (response == null)
                return BadRequest("Historical data has issue");

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetHistoricalRates");
            return StatusCode(500, "Internal Server Error");
        }
    }
}
