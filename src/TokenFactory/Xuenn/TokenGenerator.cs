using AgileBet.Cash.Integration.Utilties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TokenFactory.Xuenn
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly TokenGenerateOption _option;

        public TokenGenerator(TokenGenerateOption option)
        {
            _option = option;
        }
        public string GenerateToken(List<Claim> claims)
        {

            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in claims)
                {
                    sb.Append(item.Value + "|");
                }
                //Generate raw token
                var strRawToken = sb.ToString() + DateTimeEncoder.EncryptTimeStamp(DateTime.Now.AddSeconds(_option.Expirty));

                //Encrypt token and return
                return Encryptor.EncryptAES(strRawToken, _option.CommonCSNTokenKey, _option.CommonCSNTokenIV).Replace('+', '-').Replace('/', '_').Replace('=', '.');
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}
