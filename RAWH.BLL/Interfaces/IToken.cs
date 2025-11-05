using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RAWH.DAL.Data;

namespace RAWH.BLL.Interfaces
{
    public interface IToken
    {
         string GenerateJwtToken(ApplicationUser user);
    }
}
