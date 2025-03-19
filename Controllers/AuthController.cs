using currency_converter.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using currency_converter.Models;
using Microsoft.Extensions.Logging;

namespace currency_converter.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(TokenService tokenService, ILogger<AuthController> logger) 
        { 
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginPara request)
        {
            try
            {
                _logger.LogInformation($"Request received {HttpContext.Request.Path}");

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (request.Username == "admin" && request.Password == "admin123")
                {
                    var token = _tokenService.GenerateToken(request.Username, "Admin", request.ClientId);
                    return Ok(new { Token = token });
                }
                else if (request.Username == "user" && request.Password == "user123")
                {
                    var token = _tokenService.GenerateToken(request.Username, "User", request.ClientId);
                    return Ok(new { Token = token });
                }

                return Unauthorized(new { Message = "Invalid credentials" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while making login attemp");
                return BadRequest("Something went wrong while making login attemp");
            }
        }
    }
}
