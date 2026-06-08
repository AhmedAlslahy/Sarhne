namespace Sarhne.BLL.DTOs.Token
{
    public class GenerateTokenResDto
    {
        public required string Token { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}