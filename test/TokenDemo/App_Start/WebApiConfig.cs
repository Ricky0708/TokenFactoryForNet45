using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TokenDemo.Attributes;
using TokenDemo.Services;
using TokenFactory;
using TokenFactory.JWT;
using TokenFactory.Models;

namespace TokenDemo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            Func<HttpRequest, TokenInfo> tokenFromHeader = request => new TokenInfo() { Token = request.Headers.GetValues("Authorization")?.FirstOrDefault()?.Substring("Bearer ".Length) };
            Func<HttpRequest, TokenInfo> tokenFromQueryString = request => new TokenInfo() { Token = request.QueryString.GetValues("Bearer")?.FirstOrDefault() };
            var tokenValidator = new TokenValidator(new TokenValidateOption()
            {
                ValidIssuer = "RickyServer",
                ValidAudience = "RickyClient",
                IssuerSigningKey = "AAAAAAAAAAAAAAAAAAAAA"
            });
            config.MessageHandlers.Add(new ProcessTokenMessageHandler(tokenValidator, tokenFromHeader, (v, t) =>
            {
                if (v.IsValid)
                {
                    HttpContext.Current.User = v.Principal;
                }
                else
                {
                    var message = v.ErrorMsg;
                    // TODO LOG message
                }
                foreach (var item in t?.ExteneralInfo ?? new Dictionary<string, string>())
                {
                    HttpContext.Current.Items[item.Key] = item.Value;
                }

            }));
            config.MessageHandlers.Add(new ProcessTokenMessageHandler(tokenValidator, tokenFromQueryString, (v, t) =>
            {
                if (v.IsValid)
                {
                    HttpContext.Current.User = v.Principal;
                    HttpContext.Current.Items["CustomToken"] = t.Token;
                }
                else
                {
                    var message = v.ErrorMsg;
                    // TODO LOG message
                }
                foreach (var item in t?.ExteneralInfo ?? new Dictionary<string, string>())
                {
                    HttpContext.Current.Items[item.Key] = item.Value;
                }
            }));

            config.Filters.Add(new MyAuthorize());
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
