using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenFactory;
using TokenFactory.JWT;

namespace TokenDemo.Services
{
    public interface ITokenService
    {
        string GenerateToken(List<Claim> claims);
    }

    public class TokenServce : ITokenService
    {
        private ITokenGenerator _generator;
        public TokenServce()
        {
            // 設定制作token的參數, INTG 應該不用
            _generator = new TokenGenerator(new TokenGenerateOption()
            {
                Audience = "RickyClient",
                Issuer = "RickyServer",
                ExpireSeconds = 120000,
                IssuedAt = DateTime.Now,
                Secret = "AAAAAAAAAAAAAAAAAAAAA"
            });
        }

        /// <summary>
        /// 產生 Token, INTG 應該不用
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public string GenerateToken(List<Claim> claims)
        {
            return _generator.GenerateToken(claims);
        }


    }
}
