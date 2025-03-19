using System.Text.Json;
using currency_converter.IServices;
using currency_converter.Models;
using Microsoft.Extensions.Caching.Memory;

namespace currency_converter.Services
{
    public class FrankFurterService : IExchangeRateService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<FrankFurterService> _logger;
        private readonly string baseUrl = "https://api.frankfurter.app";

        public FrankFurterService(HttpClient httpClient, IMemoryCache memoryCache, ILogger<FrankFurterService> logger)
        {
            _httpClient = httpClient;
            _memoryCache = memoryCache;
            _logger = logger;
        }

        private static readonly string[] ExcludeCurrencies = new[] { "TRY", "PLN", "THB", "MXN" };

        public async Task<Currency> GetLatestRates(string baseCurrency)
        {
            try
            {
                if (_memoryCache.TryGetValue(baseCurrency, out Currency cachedRates))
                    return cachedRates;

                var response = await _httpClient.GetAsync($"{baseUrl}/latest?base={baseCurrency}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve exchange rates for {baseCurrency}");
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync();
                var exchangeRates = JsonSerializer.Deserialize<Currency>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _memoryCache.Set(baseCurrency, exchangeRates, TimeSpan.FromMinutes(10));

                return exchangeRates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching latest exchange rate");
                return null;
            }
        }

        public async Task<decimal?> ConvertCurrency(string from, string to, decimal amount)
        {
            try
            {
                if (ExcludeCurrencies.Contains(from.ToUpper()) || ExcludeCurrencies.Contains(to.ToUpper()))
                {
                    _logger.LogWarning($"Invalid currencies are involved");
                    return null;
                }

                var rates = await GetLatestRates(from);
                if (rates == null || !rates.Rates.ContainsKey(to))
                {
                    _logger.LogError($"Invalid currencies are involved, from {from} and to {to}");
                    return null;
                }

                return amount * rates.Rates[to];
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in currnecy conversion");
                return null;
            }
        }

        public async Task<PaginatedResponse<HistoricalRateResponse>> GetHistoricalExchangeRates(DateTime startDate, DateTime endDate, string baseCurrency, int page, int pageSize)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{baseUrl}/{startDate:yyyy-MM-dd}..{endDate:yyyy-MM-dd}?from={baseCurrency}");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"Failed to retrieve historical exchange rates for {baseCurrency}");
                    return null;
                }

                var ratesData = await response.Content.ReadFromJsonAsync<HistoricalRateResponse>();

                var pagedRates = ratesData.Rates
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToDictionary(k => k.Key, v => v.Value);

                return new PaginatedResponse<HistoricalRateResponse>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = ratesData.Rates.Count,
                    Data = new HistoricalRateResponse { Rates = pagedRates }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching historical exchange rate");
                return null;
            }
        }
    }
}
