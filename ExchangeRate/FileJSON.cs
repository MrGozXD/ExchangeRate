using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class FileJSON : File
    {
        public FileJSON(DateTime date) : base(date) 
        {
            this.Extension = ".json";
        }

        public override async Task WriteAsync(List<ExchangeRateService.RateResponse> rateResponses)
        {
            /* JSON Output structure
             * {
             *   "fromTo": "$(base)$(quote)",
             *   "rate": $(rate)
             * }
             */
            var output = rateResponses.Select(rateResponse => new
            {
                fromTo = $"{rateResponse.Base}{rateResponse.Quote}",
                rate = rateResponse.Rate
            });
            var json = System.Text.Json.JsonSerializer.Serialize(output, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            await System.IO.File.WriteAllTextAsync(this.FullFilePath, json);
        }
    }
}
