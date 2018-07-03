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
            if (parameters == null || String.IsNullOrEmpty(parameters.IssuerSigningKey))
            {
                throw new ArgumentException("options or secret can't be null");
            }
            _parameters = parameters;
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
            catch (SecurityTokenInvalidIssuerException ex)
            {
                isValidate = false;
                errorMsg = ex.Message;
                principal = null;
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                isValidate = false;
                errorMsg = ex.Message;
                principal = null;
            }
            catch (SecurityTokenExpiredException ex)
            {
                isValidate = false;
                errorMsg = ex.Message;
                principal = null;
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
                ValidateLifetime = _parameters.ValidateLifetime,
                ValidIssuer = _parameters.ValidIssuer,
                ValidAudience = _parameters.ValidAudience,
                //ValidateIssuerSigningKey = _parameters.ValidateIssuerSigningKey,
            };
            var result = func(token, paras);
            return result;

        }
    }
}
