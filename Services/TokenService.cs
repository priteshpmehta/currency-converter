using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using currency_converter.IServices;
using Microsoft.IdentityModel.Tokens;

namespace currency_converter.Services
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly string _jwtKey; 
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryMinutes;

        public TokenService(ILogger<TokenService> logger, IConfiguration configuration)
        {
            _logger = logger;

            var jwtSection = configuration.GetSection("JwtSettings");
            _jwtKey = jwtSection["Secret"];
            _issuer = jwtSection["Issuer"];
            _audience = jwtSection["Audience"];
            _expiryMinutes = int.TryParse(jwtSection["ExpiryMinutes"], out int expiry) ? expiry : 120;
        }

        public string GenerateToken(string username, string role, string clientId)
        {
            try
            {
                //var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
                //var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                //var claims = new List<Claim>
                //{
                //    new Claim(JwtRegisteredClaimNames.Sub, username),
                //    new Claim("role", role),
                //    new Claim("clientId", clientId),
                //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                //};

                //var token = new JwtSecurityToken(
                //    issuer: _issuer,
                //    audience: _audience,
                //    claims: claims,
                //    expires: DateTime.UtcNow.AddHours(2),
                //    signingCredentials: credentials
                //);

                //return new JwtSecurityTokenHandler().WriteToken(token);

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_jwtKey);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username),
                    new Claim("clientId", clientId),
                    new Claim(ClaimTypes.Role, role),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddHours(2),
                    Issuer = _issuer,
                    Audience = _audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while generating token");
                return null;
            }
        }
    }
}
