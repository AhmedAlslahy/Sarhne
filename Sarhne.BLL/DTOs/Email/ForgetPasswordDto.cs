

namespace Sarhne.BLL.DTOs.Email
{
    public class ForgetPasswordDto
    {
        public string UserId { get; set; }
        public string OTP { get; set; }
        public string NewPassword { get; set; }
    }
}
