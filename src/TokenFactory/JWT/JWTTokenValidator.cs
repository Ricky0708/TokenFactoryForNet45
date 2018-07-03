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
    public class JWTTokenValidator : ITokenValidator
    {
        private JWTTokenValidateOption _parameters;
        public JWTTokenValidator(string secret)
        {
            _parameters = new JWTTokenValidateOption()
            {
                 IssuerSigningKey = secret
            };
        }
        public JWTTokenValidator(JWTTokenValidateOption parameters)
        {
            _parameters = _parameters ?? throw new ArgumentException("parameters can't be null");
        }
        private Func<string, TokenValidationParameters, ValidationResult> func = (funcToken, funcParameters) =>
        {
            bool isValidate = false;
            string errorMsg = "";
            ClaimsPrincipal principal = null;
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                isValidate = true;
                errorMsg = "";
                principal = tokenHandler.ValidateToken(funcToken, funcParameters, out SecurityToken securityToken);
            }
            catch (Exception ex)
            {
                isValidate = false;
                errorMsg = ex.Message;
                principal = null;
            }
            return new ValidationResult()
            {
                IsValid = isValidate,
                ErrorMsg = errorMsg,
                Principal = principal
            };
        };

        /// <summary>
        /// 驗證並取得 principal
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns>true return principal, false return message</returns>
        public ValidationResult ValidateTokenAndGetPrincipal(string token)
        {
            var paras = new TokenValidationParameters()
            {
                ClockSkew = _parameters.ClockSkew,
                RequireExpirationTime = _parameters.RequireExpirationTime,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_parameters.IssuerSigningKey)),
                ValidateAudience = _parameters.ValidateAudience,
                ValidateIssuer = _parameters.ValidateIssuer,
                ValidateIssuerSigningKey = _parameters.ValidateIssuerSigningKey,
                ValidateLifetime = _parameters.ValidateLifetime,
                ValidIssuer = _parameters.ValidIssuer,
                ValidAudience = _parameters.ValidAudience,
            };
            var result = func(token, paras);
            return result;

        }
    }
}
