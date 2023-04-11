using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NationalityAPI
{
    public class Name
    {
        public string first { get; set; }
        public string last { get; set; }
    }

    public class Country
    {
        public string country_id { get; set; }
        public double probability { get; set; }
    }

    public class NationalityInfo
    {
        public Name name { get; set; }
        public List<Country> country { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string url = "https://api.nationalize.io/?name=john";

        static async Task Main(string[] args)
        {
            await getNationalityInfo();
            Console.ReadKey();

            async Task getNationalityInfo()
            {
                Console.WriteLine("Getting JSON...");
                var responseString = await client.GetStringAsync(url);
                Console.WriteLine("Parsing JSON...");
 
           NationalityInfo nationalityInfo =
                   JsonSerializer.Deserialize<NationalityInfo>(responseString);
                Console.WriteLine($"First name: {nationalityInfo?.name?.first}");
                Console.WriteLine($"Last name: {nationalityInfo?.name?.last}");
                Console.WriteLine("Countries:");
                foreach (var country in nationalityInfo?.country)
                {
                    Console.WriteLine($"{country?.country_id} ({country?.probability})");
                }
            }
        }
    }
}
