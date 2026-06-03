
using Microsoft.AspNetCore.Http;
using Sarhne.DAL.Enums;

namespace Sarhne.BLL.DTOs.User
{
    public class UserUpdateDto
    {
        public string? FullName { get; set; }
        public string? PhoneNumber { get; set; } 
        public Gender? Gender { get; set; }
        public string? ProfileDescription { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image {  get; set;}
        public string? PublicLink { get; set; } 
    }
}
