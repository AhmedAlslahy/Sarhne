namespace Sarhne.BLL.DTOs.Email
{
    public class ForgetPasswordDto
    {
        public required string OTP { get; set; }
        public required string NewPassword { get; set; }
    }
}