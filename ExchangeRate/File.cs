using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate
{
    public abstract class File
    {
		private string baseFileName;

		public string BaseFileName
		{
			get { return baseFileName; }
			set { baseFileName = value; }
		}

		private string extension;

		public string Extension
		{
			get { return extension; }
			set { extension = value; }
		}

		public string FullFileName
		{
			get { return baseFileName + extension; }
		}

		public static string GetProjectRoot()
		{
            // l'executable est dans bin/Debug/net8.0, on remonte donc de 3 niveaux pour atteindre le répertoire racine du projet
            return System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..", "..", "..")); 
        }

        public string FullFilePath
		{
			get { return System.IO.Path.Combine(GetProjectRoot(), FullFileName); }
        }
        
        public File(DateTime date)
		{
			this.BaseFileName = "Cotations-" + date.ToString("yyyy-MM-dd").Replace("-", "");
            this.Extension = ".txt";
        }

		public abstract Task WriteAsync(List<ExchangeRateService.RateResponse> rateResponses);

    }
}
