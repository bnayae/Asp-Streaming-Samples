using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppStreamingCLient
{
    class Program
    {
        private const string URL = "http://localhost:6566/api/values/file";

        static async Task Main(string[] args)
        {
            using (var http = new HttpClient())
            {
                var srm = await http.GetStreamAsync(URL);
                using (var r = new StreamReader(srm))
                {
                    bool stepByStep = true;
                    while (!r.EndOfStream)
                    {
                        var line = await r.ReadLineAsync();
                        Console.WriteLine(line);
                        if (stepByStep)
                        {
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                                stepByStep = false;
                        }
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
