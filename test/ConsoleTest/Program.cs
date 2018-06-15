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
            ITokenGenerator manager = new JWTTokenGenerator(new JWTTokenGenerateOption()
            {
                Audience = "Test",
                Issuer = "Test",
            });
            ITokenValidator validator = new JWTTokenValidator(new JWTTokenValidateOption()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
            });
            var token = manager.GenerateToken(new List<Claim>()
                  {
                      new Claim("AA", "QQ"),
                  });
            Console.WriteLine(token);
            var result = validator.ValidateTokenAndGetPrincipal(token);
            Console.WriteLine(result.Principal.FindFirst("AA").Value);
            Console.WriteLine(token.Length);
            Console.ReadLine();
        }
    }
}
