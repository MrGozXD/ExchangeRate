using ExchangeRate;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var service = new ExchangeRateService();
        var rates = await service.GetExchangeRateAsync("USD");
        var date = DateTime.Today;
        var JSONWriter = new FileJSON(date);
        var CSVWriter = new FileCSV(date);
        var XMLWriter = new FileXML(date);
        var Writers = new List<ExchangeRate.File> { JSONWriter, CSVWriter, XMLWriter };


        if (rates != null)
        {
            Console.WriteLine($"Nombre de taux de change récupérés : {rates.Count}");
            foreach (var rate in rates)
            {
                Console.WriteLine($"{rate.Base} -> {rate.Quote} : {rate.Rate} (le {rate.Date})");
            }
            foreach (var writer in Writers)
            {
                try
                {
                    writer.WriteAsync(rates).Wait();
                    Console.WriteLine($"Fichier {writer.Extension.Replace(".","").ToUpper()} écrit avec succès : {writer.FullFilePath}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Erreur lors de l'écriture du fichier {writer.Extension} : {e.Message}");
                }
            }

        }
        else
        {
            Console.WriteLine("Aucun taux de change récupéré.");
        }
    }
}