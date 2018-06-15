using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TokenDemo.Services
{
    public interface IAccountManager
    {
        string UserName { get; }
        string[] Permisions { get; }
        string PlateFromId { get; }
        string Device { get; }
        decimal Balance { get; }
        string Token { get; }
    }
    public class AccountManager : IAccountManager
    {
        public AccountManager()
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;
            UserName = identity.Name;
            PlateFromId = identity.FindFirst("PlateFormId")?.Value;
            Device = identity.FindFirst("Device")?.Value;
            Permisions = identity.FindAll("Permission").Select(p => p.Value).ToArray();
            Token = HttpContext.Current.Request.Headers["Authorization"];
        }
        public string UserName { get; private set; }

        public string[] Permisions { get; private set; }

        public string PlateFromId { get; private set; }

        public string Device { get; private set; }

        public decimal Balance => 999; // get from db

        public string Token { get; private set; }
    }
}
