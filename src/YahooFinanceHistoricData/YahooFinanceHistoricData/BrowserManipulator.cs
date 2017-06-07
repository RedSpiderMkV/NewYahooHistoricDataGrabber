using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Example;

namespace YahooFinanceBrowserManipulator2
{
    public partial class BrowserManipulator : Form
    {
        public ChromiumWebBrowser _browser;
        private readonly string _symbol;
        private readonly string _name;

        public BrowserManipulator(string symbol, string name)
        {
            _symbol = symbol;
            _name = name;

            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            string url = $"https://finance.yahoo.com/quote/{_symbol}/history?period1=852076800&period2=1924905600&interval=1d&filter=history&frequency=1d";

            var settings = new CefSettings()
            {
                //By default CefSharp will use an in-memory cache, you need to specify a Cache Folder to persist data
                CachePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CefSharp\\Cache"),
                LogSeverity = LogSeverity.Disable
            };

            //Perform dependency check to make sure all relevant resources are in our output directory.
            Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);

            _browser = new ChromiumWebBrowser(url);
            panel1.Controls.Add(_browser);

            _browser.LoadingStateChanged += _browser_LoadingStateChanged;

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;

            timer.Interval = 45000;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("APPLICATION: Time out...");
            Application.Exit();
        }

        private void _browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading)
            {
                var frame = _browser.GetMainFrame();
                // Get Document Height
                var task = frame.EvaluateScriptAsync("(function() { return document.body.innerHTML; })();", null);

                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        var response = t.Result;
                        string responseStr = (string)response.Result;
                        string containsStr = $"https://query1.finance.yahoo.com/v7/finance/download/{_symbol}?period1";

                        int startStrIndex = responseStr.IndexOf(containsStr);
                        int endStrIndex = responseStr.IndexOf('"', startStrIndex);

                        string newLink = responseStr.Substring(startStrIndex, endStrIndex - startStrIndex).Replace("&amp;", "&");

                        _browser.LoadingStateChanged -= _browser_LoadingStateChanged;
                        _browser.DownloadHandler = new DownloadHandler(@"C:\Users\Nikeah\Documents\temp", $"{_name}_{_symbol}.csv");
                        ((DownloadHandler)_browser.DownloadHandler).OnDownloadUpdatedFired += Form1_OnDownloadUpdatedFired;

                        _browser.Load(newLink);
                    }
                }, TaskScheduler.Default);
            }
        }

        private void Form1_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            Console.WriteLine("APPLICATION: Download OK.");
            Application.Exit();
        }
    }
}
