namespace Sarhne.BLL.DTOs.Email
{
    public class ResetPasswordDto
    {
        public required string CurrentPassword { get; set; }
        public required string NewPassword { get; set; }
    }
}