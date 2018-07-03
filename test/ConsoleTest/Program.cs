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
            ITokenGenerator manager = new JWTTokenGenerator(new JWTTokenGenerateOption()
            {
                Audience = "Test",
                Issuer = "Test",
                Secret = "IIIIIIIIIIIIIIIIIIIIIIIIIIII",
                ExpireSeconds = 50
            });
            var token = manager.GenerateToken(new List<Claim>()
                  {
                      new Claim("Name", "Ricky"),
                      new Claim("Age", "25"),
                      new Claim("Birthday", "100/01/01"),
                  });

            // ----------驗證及取出資料----------
            ITokenValidator validator = new JWTTokenValidator(new JWTTokenValidateOption()
            {
                ValidIssuer = "Test", // if ValidateIssuer = true
                ValidAudience = "Test", // if ValidateAudience = true
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = "IIIIIIIIIIIIIIIIIIIIIIIIIIII",
            });
            var result = validator.ValidateTokenAndGetPrincipal(token);

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
