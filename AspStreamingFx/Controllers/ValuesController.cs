using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace AspStreamingFx.Controllers
{
    public class ValuesController : ApiController
    {
        private const int SIZE = 10_000;

        // GET api/values
        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            await Task.Delay(1).ConfigureAwait(false);
            var items = from i in Enumerable.Range(0, SIZE)
                        select $"\"A{i}\",{i}";
            items = new[] { "{" }.Concat(items).Concat(new[] { "}" });
            return items;
        }
    }
}
