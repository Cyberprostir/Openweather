using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WhatNationality
{
    public class Name
    {
        public string name { get; set; }
        public string country_id { get; set; }
        public double probability { get; set; }
        public int count { get; set; }
    }

    public class NationalityResponse
    {
        public IList<Name> NameList { get; set; }
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string url = "https://api.nationalize.io/?name=";

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;
            Console.WriteLine("Введіть 1-10 імен, відділених комами:");
            string input = Console.ReadLine();
            string[] names = input.Split(',');
            if (names.Length > 10)
            {
                Console.WriteLine("Ввели більше 10 імен. Лише перші 10 будуть опрацьовані.");
                names = names.Take(10).ToArray();
            }
            foreach (string name in names)
            {
                NationalityResponse response = await GetNationality(name.Trim());
                Console.WriteLine($"Name: {response?.NameList[0]?.name}");
                Console.WriteLine($"Nationality: {response?.NameList[0]?.country_id}");
                Console.WriteLine($"Probability: {response?.NameList[0]?.probability}");
            }
            Console.ReadKey();
        }

        static async Task<NationalityResponse> GetNationality(string name)
        {
            try
            {
                Console.WriteLine($"Getting nationality for {name}...");
                var responseString = await client.GetStringAsync(url + name);
                Console.WriteLine($"Parsing nationality for {name}...");
                NationalityResponse nationalityResponse =
                   JsonSerializer.Deserialize<NationalityResponse>(responseString);
                return nationalityResponse;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting nationality for {name}: {ex.Message}");
                return null;
            }
        }
    }
}
