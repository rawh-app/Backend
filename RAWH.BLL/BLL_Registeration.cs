using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        }
    }
}
