namespace Sarhne.BLL.DTOs.Auth
{
    public class LoginRes
    {
        public required string Token { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}