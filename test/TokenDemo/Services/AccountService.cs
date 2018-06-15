using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TokenFactory;

namespace TokenDemo.Services
{
    public interface IAccountService {
        List<Claim> GetClaimsByUser(string userId);
    }
    public class AccountService : IAccountService
    {
        /// <summary>
        /// 將資訊簽入Token中
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Claim> GetClaimsByUser(string userId)
        {
            return new List<Claim>() {
                new Claim(ClaimTypes.Name, userId),
                new Claim("PlateFormId", "1"),
                new Claim("Device", "2"),
                new Claim("Permission", "Admin"),
                new Claim("Permission", "Admin2"),
            };
        }
    }
}
