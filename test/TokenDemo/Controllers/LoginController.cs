using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web;
using TokenFactory;
using TokenFactory.JWT;
using System.Security.Claims;
using TokenDemo.Services;
using TokenDemo.Attributes;

namespace TokenDemo.Controllers
{
    public class LoginController : ApiController
    {
        private IAccountService _accountService;
        private ITokenService _signinService;
        private IAccountManager _accountManager;
        public LoginController()
        {
            _accountService = new AccountService();
            _signinService = new TokenServce();
            _accountManager = new AccountManager();
        }
        public IHttpActionResult POST()
        {
            var claims = _accountService.GetClaimsByUser("Ricky");
            var token = _signinService.GenerateToken(claims);
            return Ok(token);
        }

        public IHttpActionResult GET()
        {
            return Ok(new
            {
                Name = _accountManager.UserName,
                Permissions = _accountManager.Permisions,
                Balance = _accountManager.Balance
            });
        }
    }
}
