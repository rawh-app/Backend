using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration configuration;

        public AuthService(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IToken tokenService,
            IEmailService emailService,
            IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.tokenService = tokenService;
            _emailService = emailService;
            this.configuration = configuration;
        }

        // ================= REGISTER =================
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

        // ================= LOGIN =================
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

        // ================= SEND RESET CODE =================
        public async Task<AuthModelDTO> SendResetCode(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return new AuthModelDTO { message = "Email not found!" };

            var code = Random.Shared.Next(100000, 999999).ToString();

            user.resetPasswordEmail = code;
            await userManager.UpdateAsync(user);

            var htmlMessage = $@"
                <h3>Hello {user.UserName},</h3>
                <p>Use this code to reset your password:</p>
                <h2 style='color:red'>{code}</h2>";

            await _emailService.sendEmail(user.Email!, htmlMessage);

            return new AuthModelDTO
            {
                message = "Reset code sent successfully!"
            };
        }

        // ================= VERIFY RESET CODE =================
        public async Task<AuthModelDTO> VerifyResetCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return new AuthModelDTO { message = "Code is required!" };

            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.resetPasswordEmail == code);

            if (user == null)
                return new AuthModelDTO { message = "Invalid or expired code!" };

            return new AuthModelDTO
            {
                message = "Code verified successfully!"
            };
        }

        // ================= SET NEW PASSWORD =================
        public async Task<AuthModelDTO> SetNewPassword(NewPasswordDTO model)
        {
            if (model == null)
                return new AuthModelDTO { message = "Invalid request!" };

            if (model.password != model.confirmPassword)
                return new AuthModelDTO { message = "Passwords do not match!" };

            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.resetPasswordEmail != null);

            if (user == null)
                return new AuthModelDTO { message = "You must verify reset code first!" };

            var isSamePassword = await userManager.CheckPasswordAsync(user, model.password);

            if (isSamePassword)
                return new AuthModelDTO { message = "New password cannot be the same as old password!" };

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

        // ================= GOOGLE LOGIN =================
        public async Task<string> GoogleLogin(string idToken)
        {
            var clientId = configuration["GoogleAuth:ClientId"];

            if (string.IsNullOrWhiteSpace(clientId))
                throw new Exception("Google ClientId is missing in configuration");

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { clientId }
            };

            GoogleJsonWebSignature.Payload payload;

            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
            }
            catch
            {
                throw new Exception("Invalid Google ID Token");
            }

            var email = payload.Email;
            var googleId = payload.Subject;

            if (string.IsNullOrWhiteSpace(email))
                throw new Exception("Google email not found");

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    GoogleId = googleId
                };

                var result = await userManager.CreateAsync(user);

                if (!result.Succeeded)
                    throw new Exception("Failed to create Google user");
            }

            return tokenService.GenerateJwtToken(user);
        }
    }
}