using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RAWH.BLL.DTOs;
using RAWH.BLL.Interfaces;
using RAWH.DAL.Data;

namespace RAWH.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IToken tokenService;

        public AuthService(SignInManager<ApplicationUser> signInManager
                            , UserManager<ApplicationUser> userManager
                            , IToken tokenService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        //Register
        public async Task<string> Register(RegisterDTO dto)
        {

            var user = new ApplicationUser
            {
                UserName = dto.UserName,
                Email = dto.Email
            };

            var result = await userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

            return tokenService.GenerateJwtToken(user);
        }


        //Login
        public async Task<string> Login(LoginDTO dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                throw new Exception("Invalid email or password.");

            var result = await signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!result.Succeeded)
                throw new Exception("Invalid email or password.");

            return tokenService.GenerateJwtToken(user);
        }



    }
}
