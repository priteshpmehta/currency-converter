using currency_converter.Models;

namespace currency_converter.IServices
{
    public interface IExchangeRateService
    {
        Task<Currency> GetLatestRates(string baseCurrency);
        Task<decimal?> ConvertCurrency(string from, string to, decimal amount);
        Task<PaginatedResponse<HistoricalRateResponse>> GetHistoricalExchangeRates(DateTime startDate, DateTime endDate, string baseCurrency, int page, int pageSize);
    }
}
