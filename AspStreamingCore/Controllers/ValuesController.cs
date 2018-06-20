using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AspStreamingCore.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private const int SIZE = 100_000;
        private const int BUFFER_SIZE = 4_096;
        private const string JSON_CONTENT_TYPE = "application/json";// charset=utf-8";
        //private const string JSON_CONTENT_TYPE = "application/octet-stream";
        private IEnumerable<byte> _json;

        public ValuesController()
        {
            var lines = from i in Enumerable.Range(0, SIZE)
                        select $"\"A{i}\":{i},";
            lines = new[] { "{" }.Concat(lines).Concat(new[] { "\"End\":-1}" });
            _json = from line in lines
                    let buffer = Encoding.UTF8.GetBytes(line)
                    from b in buffer
                    select b;

            if (!System.IO.File.Exists("Data.json"))
            {//   System.IO.File.Delete("Data.json");
                System.IO.File.WriteAllBytes("Data.json", _json.ToArray());
            }
        }

        // GET api/values
        [HttpGet]
        [Route("file")]
        public async Task<IActionResult> GetFile()
        {
            await Task.CompletedTask.ConfigureAwait(false);// Task.Delay(1).ConfigureAwait(false);
            var fs = new System.IO.FileStream("Data.json", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read,
                                BUFFER_SIZE, true);
            return File(fs, JSON_CONTENT_TYPE);//, "data.json");
        }

        [HttpGet]
        [Route("srm")]
        public async Task<IActionResult> GetStream()
        {
            await Task.CompletedTask.ConfigureAwait(false);// Task.Delay(1).ConfigureAwait(false);
            var fs = new System.IO.FileStream("Data.json", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read,
                                    BUFFER_SIZE, true);
            var content = new StreamContent(fs, BUFFER_SIZE);
            content.Headers.ContentType = new MediaTypeHeaderValue(JSON_CONTENT_TYPE);
            //content.Headers.ContentLength = stream.GetBuffer().Length;
            var response = Ok(content);
            return response;
        }

        [HttpGet]
        [Route("live")]
        public async Task<IActionResult> GetLive()
        {
            await Task.CompletedTask.ConfigureAwait(false);// Task.Delay(1).ConfigureAwait(false);
            var srm = new LiveStream(_json);    
            return File(srm, JSON_CONTENT_TYPE);//, "data.json");
        }

        [HttpGet]
        [Route("json")]
        public async Task<IActionResult> GetJson()
        {
            await Task.CompletedTask.ConfigureAwait(false);// Task.Delay(1).ConfigureAwait(false);
            var text = Encoding.UTF8.GetString(_json.ToArray());
            var content = new StringContent(text);
            content.Headers.ContentType = new MediaTypeHeaderValue(JSON_CONTENT_TYPE);
            return Ok(content);
        }

        [HttpGet]
        [Route("video")]
        public async Task<IActionResult> GetVideo()
        {
            using (var http = new HttpClient())
            {
                var srm = await http.GetStreamAsync("https://www.sample-videos.com/video/mp4/720/big_buck_bunny_720p_10mb.mp4");
                return File(srm, "video/mp4", "GoServer.MP4");
            }
        }


        [HttpGet]
        [Route("enm")]
        public async Task<IEnumerable<byte>> GetEnumerable()
        {
            await Task.CompletedTask.ConfigureAwait(false);// Task.Delay(1).ConfigureAwait(false);
            return _json;
        }
    }
}
