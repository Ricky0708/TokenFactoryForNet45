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
        private readonly List<string> _seqKeyName;
        private readonly TokenValidateOption _option;

        public TokenValidator(TokenValidateOption option, List<string> seqKeyName)
        {
            if (seqKeyName == null || seqKeyName.Count == 0)
            {
                throw new ArgumentNullException("seqKeyName can't be empty");
            }
            _option = option;
            _seqKeyName = seqKeyName;
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
            for (int i = 0; i < _seqKeyName.Count; i++)
            {
                claimsIdentity.AddClaim(new Claim(_seqKeyName[i], array[i]));
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
