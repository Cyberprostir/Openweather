using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace NationalityAPI
{
    
    public class Country
    {
        public string country_id { get; set; }
        public double probability { get; set; }
    }

    public class NationalityInfo
    {
        public string name { get; set; }
        public List<Country> country { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static string baseUrl = "https://api.nationalize.io/?name=";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter a first name:");
            string firstName = Console.ReadLine();

            string url = baseUrl + firstName;

            await getNationalityInfo();
            Console.ReadKey();

            async Task getNationalityInfo()
            {
                Console.WriteLine("Getting JSON...");
                var responseString = await client.GetStringAsync(url);
                Console.WriteLine("Parsing JSON...");

                NationalityInfo nationalityInfo =
                       JsonSerializer.Deserialize<NationalityInfo>(responseString);
                Console.WriteLine($"First name: {nationalityInfo?.name}");
                Console.WriteLine("Countries:");
                foreach (var country in nationalityInfo?.country)
                {
                    Console.WriteLine($"{country?.country_id} ({country?.probability})");
                }
            }
        }
    }
}
