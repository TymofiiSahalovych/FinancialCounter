using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FinancialCounter.Data
{
    class CurrencyName
    {
        static string  apiKey = "8f44177184cf42aea881c9c232143c08"; // Open Exchange Rates API key

        string ApiUrl = "https://openexchangerates.org/api/currencies.json?prettyprint=false&show_alternative=false&show_inactive=false&app_id="+apiKey;
        public async Task<Dictionary<string, string>> GetCurrencyNames()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(ApiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();

                        // Deserialize JSON to Dictionary<string, string>
                        Dictionary<string, string> currencies = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                        return currencies;
                    }
                    else
                    {
                        // If API call fails, return null or handle the error
                        Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exception during API call
                Console.WriteLine($"Error occurred: {ex.Message}");
                return null;
            }
        }
    }
}

