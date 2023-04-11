using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Openweather
{
    internal class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string url = "https://api.openweathermap.org/data/2.5/forecast?lat=49.444431&lon=32.059769&appid=d2135e63e097b2cb14860a786d24da8f&units=metric&cnt=2";

        async static Task Main(string[] args)
        {
            await getWeather();
            Console.WriteLine("Hello, World!");

            async Task getWeather()
            {
                var responseString = await client.GetStringAsync(url);
                Console.WriteLine(responseString);
                Console.WriteLine("Hello");
            }

            Console.ReadKey();
        }
        
    }

}
