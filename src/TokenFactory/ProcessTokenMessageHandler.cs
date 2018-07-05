using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TokenFactory.Models;

namespace TokenFactory
{
    public delegate TokenInfo ProvideTokenDelegating(HttpRequest request);

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
        public ProcessTokenMessageHandler(ITokenValidator validator, Func<HttpRequest, TokenInfo> provideTokenFunc)
        {
            _validator = validator;
            _provideTokenDelegate = new ProvideTokenDelegating(p => provideTokenFunc(p));
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenInfo = _provideTokenDelegate(HttpContext.Current.Request);
            if (!string.IsNullOrEmpty(tokenInfo.Token))
            {
                var result = _validator.Validate(tokenInfo.Token);
                if (result.IsValid)
                {
                    HttpContext.Current.User = result.Principal;
                    HttpContext.Current.Items["CustomToken"] = tokenInfo.Token;

                }
                else
                {
                    _errorProcess?.Invoke(result.ErrorMsg);
                }
            }
            foreach (var item in tokenInfo?.ExteneralInfo ?? new Dictionary<string, string>())
            {
                HttpContext.Current.Items[item.Key] = item.Value;
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
