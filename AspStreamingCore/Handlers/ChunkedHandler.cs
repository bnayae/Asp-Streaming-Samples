using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AspStreamingCore
{
    public class ChunkedHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            if (response.RequestMessage.RequestUri.LocalPath.EndsWith("/enm", StringComparison.CurrentCulture))
            {
                response.Headers.TransferEncodingChunked = true;
            }
            return response;
        }
    }

}