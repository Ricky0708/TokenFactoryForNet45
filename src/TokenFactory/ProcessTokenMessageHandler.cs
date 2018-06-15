using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace TokenFactory
{
    public delegate string ProvideTokenDelegating(HttpRequest request);

    public class ProcessTokenMessageHandler : DelegatingHandler
    {
        private readonly ITokenValidator _validator;
        private readonly ProvideTokenDelegating _provideTokenDelegate;
        public ProcessTokenMessageHandler(ITokenValidator validator, ProvideTokenDelegating provideDelegate)
        {
            _validator = validator;
            _provideTokenDelegate = provideDelegate;
        }
        public ProcessTokenMessageHandler(ITokenValidator validator, Func<HttpRequest, string> provideTokenFunc)
        {
            _validator = validator;
            _provideTokenDelegate = new ProvideTokenDelegating(p => provideTokenFunc(p));
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _provideTokenDelegate(HttpContext.Current.Request);
            if (!string.IsNullOrEmpty(token))
            {
                var result = _validator.ValidateTokenAndGetPrincipal(token);
                if (result.IsValid)
                {
                    HttpContext.Current.User = result.Principal;
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
