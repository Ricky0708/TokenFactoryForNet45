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
            var userCode = TokenHelper.GetUserCodeFromToken(token, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV);
            var partnerId = TokenHelper.GetPartnerId(token, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV);
            var platformId = TokenHelper.GetPlatformIdFromToken(token, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV);
            var claimsIdentity = new ClaimsIdentity();
            claimsIdentity.AddClaim(new Claim("UserCode", userCode));
            claimsIdentity.AddClaim(new Claim("PartnerId", partnerId));
            claimsIdentity.AddClaim(new Claim("PlatformId", platformId));
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
