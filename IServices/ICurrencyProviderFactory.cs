namespace currency_converter.IServices
{
    public interface ICurrencyProviderFactory
    {
        IExchangeRateService GetProvider(string providerName);
    }
}
