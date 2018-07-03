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
                Secret = "IIIIIIIIIIIIIIIIIIIIIIIIIIII",
                ExpireSeconds = 50
            });
            var token = manager.GenerateToken(new List<Claim>()
                  {
                      new Claim("Name", "Ricky"),
                      new Claim("Age", "25"),
                      new Claim("Birthday", "100/01/01"),
                  });
            Console.WriteLine(token);


            ITokenValidator validator = new JWTTokenValidator(new JWTTokenValidateOption()
            {
                ValidAudience = "Test1",
                ValidIssuer = "Test2",
                ValidateAudience = true,
                ValidateIssuer = false,
                IssuerSigningKey = "IIIIIIIIIIIIIIIIIIIIIIIIIIII",
            });
            System.Threading.Thread.Sleep(6000);
            var result = validator.ValidateTokenAndGetPrincipal(token);
            Console.WriteLine("Name:" + result.Principal.FindFirst("Name").Value);
            Console.WriteLine("Age:" + result.Principal.FindFirst("Age").Value);
            Console.WriteLine("Birthday:" + result.Principal.FindFirst("Birthday").Value);
            Console.WriteLine(token.Length);
            Console.ReadLine();
        }
    }
}
