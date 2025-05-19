using AuthenService.Application.DTOs;
using AuthenService.Application.Interfaces;
using AuthenService.Domain.Entities;
using AuthenService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenService.Application.Services
{
    public class UserService: IUserService
    {
      
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;
        public UserService( UserManager<User> userManager, ILogger<UserService> logger   )
        {
          
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(RegisterUserDto registerDto)
        {
            var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists");
            }

            var user = new User
            {
               
                Email = registerDto.Email,
                UserName = registerDto.Username,
                CreatedAt = DateTime.UtcNow,
                EmailConfirmed = true
       
            };

            var userCreationResponse = await _userManager.CreateAsync(user, registerDto.Password);
            // AN ERROR OCCURED WHILE CREATING USER
            if (!userCreationResponse.Succeeded)
            {
                _logger.LogError("User was not created successfully because : {Error} ", userCreationResponse.Errors.First());
                throw new Exception("User was not created successfully because : " + userCreationResponse.Errors.First().Description);
            }

            await _userManager.AddToRoleAsync(user, "User");
           
            return true;
        }
    }

}
