namespace currency_converter.IServices
{
    public interface ITokenService
    {
        public string GenerateToken(string username, string role, string clientId);
    }
}
