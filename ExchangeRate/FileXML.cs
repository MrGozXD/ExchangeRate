using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRate
{
    public class FileXML : File
    {
        public FileXML(DateTime date) : base(date) 
        {
            this.Extension = ".xml";
        }

        [XmlRoot("Rates")]
        public class RatesOutput
        {
            [XmlElement("Rate")]
            public List<RateItem> RateItems { get; set; } = new List<RateItem>();

        }

        public class RateItem
        {
            [XmlElement("fromTo")]
            public string FromTo { get; set; } = string.Empty;

            [XmlElement("rate")]
            public decimal Rate { get; set; }
        }
        public override async Task WriteAsync(List<ExchangeRateService.RateResponse> rateResponses)
        {
            /* XML Output structure
             * <Rates>
             *   <Rate fromTo="$(base)$(quote)" rate="$(rate)" />
             * </Rates>
             */
            var output = new RatesOutput
                {
            RateItems = rateResponses.Select(rateResponse => new RateItem
            {
                FromTo = $"{rateResponse.Base}{rateResponse.Quote}",
                Rate = rateResponse.Rate
            }).ToList() };
                
            var serializer = new XmlSerializer(typeof(RatesOutput));

            await using var stream = System.IO.File.Create(this.FullFilePath);
            serializer.Serialize(stream, output);
        }
    }
}
