using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using TokenDemo.Services;

namespace TokenDemo.Attributes
{
    public class MyAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Count != 0)
            {
                return;
            }
            
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
                actionContext.Response.Content = new StringContent(JsonConvert.SerializeObject(new
                {
                    Error = "Error"
                }));
            }
        }
    }
}
