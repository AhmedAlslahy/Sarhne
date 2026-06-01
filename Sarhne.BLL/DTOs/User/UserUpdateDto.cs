
using Microsoft.AspNetCore.Http;

namespace Sarhne.BLL.DTOs.User
{
    public class UserUpdateDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; } 
        public string? ProfileDescription { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image {  get; set;}
        public string PublicLink { get; set; } 
    }
}
