using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;

namespace currency_converter.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();
            var request = context.Request;

            // Capture Client IP
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            // Extract ClientId from JWT Token (if present)
            string clientId = "Unknown";
            var authorizationHeader = request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length);
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;
                clientId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "clientId")?.Value ?? "Unknown";
            }

            await _next(context);
            stopwatch.Stop();

            // Capture Response Code
            var responseCode = context.Response.StatusCode;
            var responseTime = stopwatch.ElapsedMilliseconds;

            // Log Details
            _logger.LogInformation($"Request: ClientIP={clientIp}, ClientId={clientId}, Method={request.Method}, Endpoint={request.Path}, " +
                $"ResponseCode={responseCode}, ResponseTime={responseTime}ms");
        }
    }
}
