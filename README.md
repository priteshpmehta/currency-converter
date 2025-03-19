# Currency Converter API
Technical Assessment for Senior ‌‌‌‌Senior Backend (C#) Developer‌‌‌

This is a robust, scalable, and maintainable Currency Conversion API built using C# and ASP.NET Core 8.0. It fetches real-time exchange rates, converts between currencies, and supports historical data with pagination. The API is designed with resilience, security, and extensibility in mind.


# 🚀 **Features**

- **Retrieve Latest Exchange Rates**: Get real-time exchange rates for a given base currency.
- **Currency Conversion**: Convert amounts between different currencies (excluding TRY, PLN, THB, and MXN).
- **Historical Exchange Rates with Pagination**: Fetch historical rates within a date range.
- **Caching, Resilience & Retry Mechanisms**: Uses Polly for circuit breakers and retries.
- **Security & Authentication**: JWT-based authentication with role-based access control.
- **Logging & Monitoring**: Uses structured logging for distributed tracing.
- **Swagger UI**: API documentation with JWT authentication enabled.
- **API Versioning**: Ensures backward compatibility.
- **Multiple Exchange Rate Providers**: Support for multiple providers.

# 📂 Project Structure

```📦 currency-converter-api
 ┣ 📂 Controllers      # API controllers
 ┣ 📂 IServices        # Interfaces
 ┣ 📂 Services         # Business logic
 ┣ 📂 Models           # Data models
 ┣ 📂 Middleware       # Custom middleware for logging
 ┣ 📜 Program.cs       # Main entry point, DI setup, middleware registration
 ┣ 📜 appsettings.json # Configuration file
```

# 🛠 Setup Instructions
**Prerequisites**
Ensure you have the following installed:
- .NET 8.0 SDK
- Visual Studio 2022 / VS Code

# Steps to Run Locally
- Clone the repository
    ```
    git clone https://github.com/priteshpmehta/currency-converter.git
    cd currency-converter
    ```
- Set up environment variables (or modify appsettings.json accordingly).
- Restore dependencies:
    ```
    dotnet restore
    ```
- Run the application:
    ```
    dotnet run
    ```
- Access API Documentation (Swagger UI): 
    - Open your browser and navigate to: `http://localhost:5000/swagger`

# 📌 Assumptions Made 
- The Frankfurter API is the primary exchange rate provider.
- The application will exclude TRY, PLN, THB, and MXN from conversions.
- Default base currency is EUR unless specified otherwise.
- API consumers will authenticate via JWT tokens.
- The circuit breaker policy prevents API overload during failures.

# 🚀 Future Enhancements
- **Database Integration**: Store exchange rates for historical analysis and caching.
- **Rate Limiting**: Implement request throttling per user.
- **Unit Test**: Integration tests to verify API interactions and test reports.
- **CI/CD Pipeline**: Automate deployments using GitHub Actions.

# Author
- Pritesh Mehta

