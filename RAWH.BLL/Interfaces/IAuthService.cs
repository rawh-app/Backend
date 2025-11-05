using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAWH.BLL.DTOs;

namespace RAWH.BLL.Interfaces
{
    public interface IAuthService
    {
        Task<string> Login(LoginDTO dto);
        Task<string> Register(RegisterDTO dto);

    }
}
