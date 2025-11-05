using Microsoft.Extensions.DependencyInjection;
using RAWH.BLL.Interfaces;
using RAWH.BLL.Services;

namespace RAWH.BLL
{
    public static class BLL_Registeration
    {
        public static void ADDBLL_Registeration(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();

            services.AddScoped<IToken, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
        }
    }
}
