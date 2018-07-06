using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using TokenFactory.Models;

namespace TokenFactory
{
    public delegate TokenInfo ProvideTokenDelegating(HttpRequest request);
    public delegate void ProvideTokenInfoCallBackDelegating(ValidationResult result, TokenInfo tokenInfo);
    public class ProcessTokenMessageHandler : DelegatingHandler
    {
        private readonly ITokenValidator _validator;
        private readonly ProvideTokenDelegating _provideTokenDelegate;
        private readonly ProvideTokenInfoCallBackDelegating _provideTokenInfoCallBackDelegate;
        private readonly Action<string> _errorProcess;
   
        public ProcessTokenMessageHandler(ITokenValidator validator, Func<HttpRequest, TokenInfo> provideTokenFunc, Action<ValidationResult, TokenInfo> provideTokenInfoAct)
        {
            _validator = validator;
            _provideTokenDelegate = new ProvideTokenDelegating(p => provideTokenFunc(p));
            _provideTokenInfoCallBackDelegate = new ProvideTokenInfoCallBackDelegating((p, t) => provideTokenInfoAct(p, t));
        }
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var tokenInfo = _provideTokenDelegate(HttpContext.Current.Request);
            if (!string.IsNullOrEmpty(tokenInfo.Token))
            {
                var result = _validator.Validate(tokenInfo.Token);
                _provideTokenInfoCallBackDelegate(result, tokenInfo);
            }
            //foreach (var item in tokenInfo?.ExteneralInfo ?? new Dictionary<string, string>())
            //{
            //    HttpContext.Current.Items[item.Key] = item.Value;
            //}
            return base.SendAsync(request, cancellationToken);
        }
    }
}
