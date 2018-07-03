using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.JWT
{
    public class JWTTokenGenerator : ITokenGenerator
    {
        private JWTTokenGenerateOption _options;
        public JWTTokenGenerator(string secret)
        {
            _options = new JWTTokenGenerateOption()
            {
                Secret = secret
            };
        }
        public JWTTokenGenerator(JWTTokenGenerateOption options)
        {
            if (options == null || String.IsNullOrEmpty(options.Secret))
            {
                throw new ArgumentException("options or secret can't be null");
            }
            _options = options;
        }
        /// <summary>
        /// Create JWT token
        /// </summary>
        /// <param name="options">Generate options</param>
        /// <returns></returns>
        public string GenerateToken(List<Claim> claims)
        {
            //assign default options
            if (_options == null)
            {
                _options = new JWTTokenGenerateOption();
            }
            _options.Subject = claims;

            var tokenDescriptor = MakeTokenDescriptor(_options);

            var tokenHandler = new JwtSecurityTokenHandler();

            var stoken = tokenHandler.CreateToken(tokenDescriptor);

            var tokenWzScheme = tokenHandler.WriteToken(stoken);

            return tokenWzScheme;
        }


        private SecurityTokenDescriptor MakeTokenDescriptor(JWTTokenGenerateOption options)
        {
            var result = func(options);
            return result;
        }

        private Func<JWTTokenGenerateOption, SecurityTokenDescriptor> func = (funOption) =>
        {
            //convert signture to base64
            var symmetricKey = Encoding.UTF8.GetBytes(funOption.Secret);

            //Generate ClaimsIdentity
            var claims = new ClaimsIdentity();
            if (funOption.Subject != null)
            {
                claims.AddClaims(funOption.Subject);
            }
            if (!funOption.IssuedAt.HasValue)
            {
                funOption.IssuedAt = DateTime.UtcNow;
            }
            //make SecurityTokenDescriptor
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = funOption.IssuedAt,
                NotBefore = funOption.NotBefore,
                Issuer = funOption.Issuer,
                Audience = funOption.Audience,
                Subject = claims,
                Expires = funOption.IssuedAt.Value.AddSeconds(Convert.ToInt32(funOption.ExpireSeconds)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };
            return tokenDescriptor;
        };
    }
}
