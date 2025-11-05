using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
        private readonly IEmailService _emailService;

        public AuthService(SignInManager<ApplicationUser> signInManager
                            , UserManager<ApplicationUser> userManager
                            , IToken tokenService
                            , IEmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
            _emailService = emailService;
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
        public async Task<AuthModelDTO> SendResetCode(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return new AuthModelDTO { message = "Email not found!" };

            var code = new Random().Next(100000, 999999).ToString();
            user.resetPasswordEmail = code;
            await userManager.UpdateAsync(user);

            var htmlMessage = $@"
        <h3>Hello {user.UserName},</h3>
        <p>Use this code to reset your password:</p>
        <h2 style='color:#ff0000'>{code}</h2>";

            await _emailService.sendEmail(user.Email, htmlMessage);

            return new AuthModelDTO
            {
                message = "Reset code has been sent successfully!"
            };
        }
        public async Task<AuthModelDTO> VerifyResetCode(string code)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.resetPasswordEmail == code);
            if (user == null)
                return new AuthModelDTO { message = "Invalid or expired code!" };
            return new AuthModelDTO
            {
                message = "Code verified successfully!"
            };
        }
        public async Task<AuthModelDTO> SetNewPassword(NewPasswordDTO model)
        {
            if (model.password != model.confirmPassword)
                return new AuthModelDTO { message = "Passwords do not match!" };
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.resetPasswordEmail != null);
            if (user == null)
                return new AuthModelDTO { message = "You must verify your code first!" };

            var samePassword = await userManager.CheckPasswordAsync(user, model.password);
            if (samePassword)
                return new AuthModelDTO { message = "New password cannot be the same as the old password!" };

            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthModelDTO { message = errors };
            }
            user.resetPasswordEmail = null;
            await userManager.UpdateAsync(user);

            return new AuthModelDTO
            {
                message = "Password changed successfully!"
            };
        }



    }
}
