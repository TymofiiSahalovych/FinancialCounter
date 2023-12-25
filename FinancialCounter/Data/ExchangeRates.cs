using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinancialCounter.Data
{
    class ExchangeRates
    {
        string apiKey = "8f44177184cf42aea881c9c232143c08"; // Open Exchange Rates API key

        public async Task<Dictionary<string, decimal>> GetLatestDataAsync()
        {
            string apiUrl = $"https://openexchangerates.org/api/latest.json?app_id={apiKey}";
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        // Deserialize JSON response
                        JObject jsonObject = JObject.Parse(result);
                        // Extract exchange rates
                        JObject rates = (JObject)jsonObject["rates"];
                        // Convert rates to Dictionary
                        Dictionary<string, decimal> exchangeRates = rates.ToObject<Dictionary<string, decimal>>();
                        return exchangeRates;
                    }
                    else
                    {
                        Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return null;
                }
            }    
        }
    }
}

