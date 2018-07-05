using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenFactory;
using TokenFactory.JWT;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // ----------製作 Token----------
            ITokenGenerator manager = new TokenGenerator(new TokenGenerateOption()
            {
                Audience = "RickyClient",
                Issuer = "RickyServer",
                Secret = "AAAAAAAAAAAAAAAAAAAAA",
                ExpireSeconds = 500000000
            });
            var token = manager.GenerateToken(new List<Claim>()
                  {
                      new Claim("UserCode", "Ricky"),
                      new Claim("CurrencyCode", "TWD"),
                      new Claim("CountryCode", "TW"),
                      new Claim("Gender", "M"),
                      new Claim("Birthdate", "100/01/01"),
                  });

            // ----------驗證及取出資料----------
            ITokenValidator validator = new TokenValidator(new TokenValidateOption()
            {
                ValidIssuer = "Test", // if ValidateIssuer = true
                ValidAudience = "Test", // if ValidateAudience = true
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = "XuennXuennXuenn1",
            });
            var result = validator.Validate(token);

            if (result.IsValid)
            {
                Console.WriteLine("Name:" + result.Principal.FindFirst("Name").Value);
                Console.WriteLine("Age:" + result.Principal.FindFirst("Age").Value);
                Console.WriteLine("Birthday:" + result.Principal.FindFirst("Birthday").Value);
            }
            else
            {
                Console.WriteLine(result.ErrorMsg);
            }

            Console.ReadLine();
        }
    }
}
