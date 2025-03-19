using System.ComponentModel.DataAnnotations;

namespace currency_converter.Models
{
    public class LoginPara
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "ClientId is required.")]
        public string ClientId { get; set; }
    }
}
