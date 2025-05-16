using AuthenService.Application.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenService.Application.DTOs
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RegisterUserDto
    {
        [Required(ErrorMessage = "Please Provide Email"), RegularExpression(RegexConstants.EmailRegex, ErrorMessage = "Please Provide A Valid Email Address.")]
        [StringLength(100, ErrorMessage = "Email Cannot Be More Than Length Of 100")]
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
    }

}
