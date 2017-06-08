using System;
using System.Windows.Forms;

namespace YahooFinanceHistoricData
{
    internal class DataGrabberMain
    {
        internal static void Main(string[] args)
        {
            string symbol = args[0];
            string name = args[1];
            string downloadDirectory = @"C:\Users\Nikeah\Documents\temp";

            Console.WriteLine("Yahoo Finance Historic Data Grabber v0.1");

            using (var yahooBrowser = new YahooBrowser(symbol, name, downloadDirectory))
            {
                Application.Run(new BrowserHost(yahooBrowser));
            }
        }
    }
}
