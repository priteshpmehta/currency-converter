namespace currency_converter.Models
{
    public class Currency
    {
        public decimal Amount { get; set; }
        public string Base { get; set; }
        public DateOnly Date { get; set; }
        public Dictionary<string, decimal> Rates { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public T Data { get; set; }
    }

    public class HistoricalRateResponse
    {
        public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }
    }
}
