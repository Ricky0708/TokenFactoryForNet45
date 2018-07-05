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
        private readonly Action<string> _errorProcess;
        public ProcessTokenMessageHandler(ITokenValidator validator, ProvideTokenDelegating provideDelegate, Action<string> errorProcess = null)
        {
            _validator = validator;
            _provideTokenDelegate = provideDelegate;
            _errorProcess = errorProcess;
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
                var result = _validator.Validate(token);
                if (result.IsValid)
                {
                    HttpContext.Current.User = result.Principal;
                    HttpContext.Current.Items["CustomToken"] = token;
                }
                else
                {
                    _errorProcess?.Invoke(result.ErrorMsg);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
