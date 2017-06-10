using CefSharp.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace YahooFinanceHistoricData
{
    internal class DataGrabberMain
    {
        private const string URL_TEMPLATE = "https://finance.yahoo.com/quote/{0}/history?period1=852076800&period2=1924905600&interval=1d&filter=history&frequency=1d";

        internal static int Main(string[] args)
        {
            string symbol = args[0];
            string name = args[1];
            string downloadDirectory = @"Data";

            if(!Directory.Exists(downloadDirectory))
            {
                Directory.CreateDirectory(downloadDirectory);
            }

            Console.WriteLine("Yahoo Finance Historic Data Grabber v0.1");
            Console.WriteLine("Downloading: " + symbol);

            int returnCode = 10;
            string fileName = $"{name}_{symbol}.csv";
            string url = string.Format(URL_TEMPLATE, symbol);

            var downloadHandler = new DownloadHandler(downloadDirectory, fileName);
            var webBrowser = new ChromiumWebBrowser(url);
            webBrowser.DownloadHandler = downloadHandler;

            using (var yahooBrowser = new YahooBrowser(symbol, webBrowser))
            {
                var host = new BrowserHost(yahooBrowser);
                Application.Run(host);

                returnCode = host.ReturnCode;
            }

            return returnCode;
        }
    }
}
