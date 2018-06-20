using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspStreamingFx
{
    public class Handler : DelegatingHandler
    {
        public static readonly Handler Default = new Handler();
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            response.Headers.TransferEncodingChunked = true;
            return response;
        }
    }

}