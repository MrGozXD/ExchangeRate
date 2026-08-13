using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public class FileCSV : File
    {
        public FileCSV(DateTime date) : base(date) 
        {
            this.Extension = ".csv";
        }
        
        public override async Task WriteAsync(List<ExchangeRateService.RateResponse> rateResponses)
        {
            var csvLines = new List<string>
            {
               "fromTo;rate" // Header
            };
            csvLines.AddRange(rateResponses.Select(rateResponse =>
                $"{rateResponse.Base}{rateResponse.Quote};{rateResponse.Rate}"));
            await System.IO.File.WriteAllLinesAsync(this.FullFilePath, csvLines);
        }
    }
}
