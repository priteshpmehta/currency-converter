using currency_converter.IServices;

namespace currency_converter.Services
{
    public class CurrencyProviderFactory : ICurrencyProviderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public CurrencyProviderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IExchangeRateService GetProvider(string providerName)
        {
            return providerName switch
            {
                "Frankfurter" => _serviceProvider.GetRequiredService<FrankFurterService>(),
                //"SecondServiceProvider" => _serviceProvider.GetRequiredService<SecondServiceProvider>();
                _ => throw new ArgumentException("Invalid provider name", providerName),
            };
        }
    }
}
