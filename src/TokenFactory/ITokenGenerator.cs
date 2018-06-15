using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory
{
    public interface ITokenGenerator
    {
        /// <summary>
        /// 產生 token
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        string GenerateToken(List<Claim> claims);
    }
}
