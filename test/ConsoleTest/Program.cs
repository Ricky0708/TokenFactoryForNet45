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
                Secret = "IIIIIIIIIIIIIIIIIIIIIIIIIIII"
            });
            ITokenValidator validator = new JWTTokenValidator(new JWTTokenValidateOption()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = "IIIIIIIIIIIIIIIIIIIIIIIIIIII",
            });
            var token = manager.GenerateToken(new List<Claim>()
                  {
                      new Claim("Name", "Ricky"),
                      new Claim("Age", "25"),
                      new Claim("Birthday", "100/01/01"),
                  });
            Console.WriteLine(token);
            var result = validator.ValidateTokenAndGetPrincipal(token);
            Console.WriteLine("Name:" + result.Principal.FindFirst("Name").Value);
            Console.WriteLine("Age:" + result.Principal.FindFirst("Age").Value);
            Console.WriteLine("Birthday:" + result.Principal.FindFirst("Birthday").Value);
            Console.WriteLine(token.Length);
            Console.ReadLine();
        }
    }
}
