using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json; 
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRate
{
    public class ExchangeRateService
    {
        private static readonly HttpClient httpClient = new HttpClient
        { 
            BaseAddress = new Uri("https://api.frankfurter.dev/v2/")
        };

        public class RateResponse
        {
            [JsonPropertyName("date")]
            [JsonPropertyOrder(1)]
            public required string Date { get; set; }

            [JsonPropertyName("base")]
            [JsonPropertyOrder(2)]
            public required string Base { get; set; }

            [JsonPropertyName("quote")]
            [JsonPropertyOrder(3)]
            public required string Quote { get; set; }

            [JsonPropertyName("rate")]
            [JsonPropertyOrder(4)]
            public required decimal Rate { get; set; }
        }


        public async Task<List<RateResponse>?> GetExchangeRateAsync(string baseCurrency)
        {
            try
            {
                var response = await httpClient.GetAsync($"rates?base={baseCurrency}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var rateResponse = JsonSerializer.Deserialize<List<RateResponse>?>(jsonResponse);
                return rateResponse;

            }
            catch (HttpRequestException exHttp)
            {
                Console.WriteLine($"Erreur lors de l'appel de l'API: {exHttp.Message}");
                return null;
            }
            catch (JsonException exJSON)
            {
                Console.WriteLine($"Erreur lors du parsing JSON: {exJSON.Message}");
                return null;
            }
        }

    }
}
