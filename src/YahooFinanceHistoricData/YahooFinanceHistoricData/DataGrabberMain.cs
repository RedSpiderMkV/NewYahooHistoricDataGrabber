using System;
using System.IO;
using System.Windows.Forms;

namespace YahooFinanceHistoricData
{
    internal class DataGrabberMain
    {
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
            using (var yahooBrowser = new YahooBrowser(symbol, name, downloadDirectory))
            {
                var host = new BrowserHost(yahooBrowser);
                Application.Run(host);

                returnCode = host.ReturnCode;
            }

            return returnCode;
        }
    }
}
