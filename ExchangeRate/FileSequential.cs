using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ExchangeRate
{
    public class FileSequential : File
    {
        public FileSequential(DateTime date) : base(date) { } // Default extension is .txt

        public override async Task WriteAsync(List<ExchangeRateService.RateResponse> rateResponses)
        {
            const char decimalSeparator = '.';

            // Rate = IntegerPart.DecimalPart
            var decomposedRates = rateResponses.Select(r =>
            {
                var rateStr = r.Rate.ToString(CultureInfo.InvariantCulture);
                var dotIndex = rateStr.IndexOf(decimalSeparator);

                var integerPart = dotIndex >= 0 ? rateStr[..dotIndex] : rateStr;
                var decimalPart = dotIndex >= 0 ? rateStr[(dotIndex + 1)..] : string.Empty;

                return new
                {
                    RateResponse = r,
                    IntegerPart = integerPart,
                    DecimalPart = decimalPart
                };
            }).ToList();

            // Longueurs max de IntegerPart et DecimalPart pour le padding
            var maxIntegerLength = decomposedRates.Max(x => x.IntegerPart.Length);
            var maxDecimalLength = decomposedRates.Max(x => x.DecimalPart.Length);

            // Construction de la string à longueur fixe
            var fixedWidthRates = decomposedRates.Select(x =>
            {
                var paddedInteger = x.IntegerPart.PadLeft(maxIntegerLength, '0');
                var paddedDecimal = x.DecimalPart.PadRight(maxDecimalLength, '0');

                var fixedStr = maxDecimalLength > 0
                    ? $"{paddedInteger}{decimalSeparator}{paddedDecimal}"
                    : paddedInteger;

                return new
                {
                    RateResponse = x.RateResponse,
                    FixedWidthRate = fixedStr
                };
            }).ToList();

            var sequentialLines = fixedWidthRates
                .Select(x => $"{x.RateResponse.Base}{x.RateResponse.Quote}{x.FixedWidthRate}")
                .ToList();

            await System.IO.File.WriteAllLinesAsync(this.FullFilePath, sequentialLines);
        }
    }
}
