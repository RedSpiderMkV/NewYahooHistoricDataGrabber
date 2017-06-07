using System;
using System.Windows.Forms;
using YahooFinanceBrowserManipulator2;

namespace YahooFinanceHistoricData
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string symbol = args[0];
            string name = args[1];

            Console.WriteLine("Yahoo Finance Historic Data Grabber v0.1");
            
            Application.Run(new BrowserManipulator(symbol, name));
        }
    }
}
