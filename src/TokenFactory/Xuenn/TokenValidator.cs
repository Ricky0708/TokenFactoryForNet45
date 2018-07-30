using AgileBet.Cash.Integration.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.Xuenn
{
    public class TokenValidator : ITokenValidator
    {
        private readonly TokenValidateOption _option;

        public TokenValidator(TokenValidateOption option)
        {
            _option = option;
        }
        public ValidationResult Validate(string token)
        {
            if (_option.IsValidExpired && !TokenHelper.Validate188Tokens(_option.validateOTT, token, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV))
            {
                return new ValidationResult()
                {
                    ErrorMsg = "Invalid token",
                    IsValid = false,
                };
            }
            var array = TokenHelper.GetTokenArray(token, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV);

            var claimsIdentity = new ClaimsIdentity();
            for (int i = 0; i < array.Length; i++)
            {
                claimsIdentity.AddClaim(new Claim(i.ToString(), array[i]));
            }
            var result = new ValidationResult()
            {
                IsValid = true,
                ErrorMsg = "",
                Principal = new ClaimsPrincipal(claimsIdentity)
            };
            return result;
        }
    }
}
