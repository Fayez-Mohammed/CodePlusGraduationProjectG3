using LearnSphere.DAL.Models.BaseModels;
using LearnSphere.Services.Interfaces;
using LearnSphere.Shared.DTOs;
using LearnSphere.Shared.DTOs.AuthDTOs;
using LearnSphere.Shared.Enums;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace LearnSphere.Services.Implementations
{
    public class AuthService : IAuthService
    {

        private readonly IUserService _userService;
        private readonly IJWTService _jWTService;
        private readonly IConfiguration _conf;
        public AuthService(IUserService userService,IJWTService jWTService,IConfiguration conf)
        {
            _userService = userService;
            _jWTService = jWTService;
            _conf = conf;
        }
        public async Task<Result<RegisterResponseDTO>> RegisterAsync(RegisterDTO registerDTO)
        {
            if (registerDTO is null)
                return Result<RegisterResponseDTO>.Failure("please fill the form");
            var existingUser=await _userService.GetByEmailAsync(registerDTO.Email);
            if(existingUser is not null)
                return Result<RegisterResponseDTO>.Failure("Email Already Exist");
            ApplicationUser user = new ApplicationUser
            {
                FullName = registerDTO.FullName,
                Email = registerDTO.Email,
                UserName=registerDTO.Email,
                UserType = UserTypes.Student

            };
           var Appuser= await _userService.CreateUserAsync(user,registerDTO.Password);
            var res = new RegisterResponseDTO
            {
                UserId = Appuser.Id,
                Email = Appuser.Email
            };
            return Result<RegisterResponseDTO>.Success(res,201);
        }

        public async Task<Result<LoginResponseDTO>> LoginAsync(LoginDTO loginDTO)
        {
            if (loginDTO is null)
                return Result<LoginResponseDTO>.Failure("please fill the form");
            var user = await _userService.GetByEmailAsync(loginDTO.Email);
            if (user is null)
                return Result<LoginResponseDTO>.Failure("there is no user with this email");
            bool isCorrect = await _userService.CheckPasswordAsync(user, loginDTO.Password);
            if (!isCorrect)
                return Result<LoginResponseDTO>.Failure("Email or password is not correct");
            string token =await _jWTService.GenerateJWTToken(user);

            if (!int.TryParse(_conf["Auth:JWT:DurationInMinuts"], out int minuts))
            {
                minuts = 60;
            }
            var result = new LoginResponseDTO
            {
                AccessToken = token,
                ExpireAt = DateTime.UtcNow.AddMinutes(minuts),
                UserLogin = new UserLoginDTO
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Role = user.UserType.ToString()
                }

            };
            return  Result<LoginResponseDTO>.Success(result);

        }
    }
}
