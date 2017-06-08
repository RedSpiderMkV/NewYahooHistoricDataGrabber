using CefSharp;
using CefSharp.Example;
using CefSharp.WinForms;
using System;
using System.Threading.Tasks;

namespace YahooFinanceHistoricData
{
    public class YahooBrowser : IDisposable
    {
        #region Events

        public event EventHandler OnDownloadUpdateComplete;

        #endregion

        #region Properties

        public ChromiumWebBrowser Browser { get { return _browser; } }

        #endregion

        #region Public Methods

        public YahooBrowser(string symbol, string name, string saveDirectory)
        {
            _name = name;
            _symbol = symbol;
            _saveDirectory = saveDirectory;

            string fileName = $"{_name}_{_symbol}.csv";
            _downloadHandler = new DownloadHandler(saveDirectory, fileName);

            string url = string.Format(URL_TEMPLATE, symbol);
            _browser = new ChromiumWebBrowser(url);
            _browser.DownloadHandler = _downloadHandler;

            _downloadHandler.OnDownloadUpdatedFired += DownloadHandler_OnDownloadUpdatedFired;
            _browser.LoadingStateChanged += Browser_LoadingStateChanged;
        }

        public void Dispose()
        {
            _downloadHandler.OnDownloadUpdatedFired -= DownloadHandler_OnDownloadUpdatedFired;
            _browser.LoadingStateChanged -= Browser_LoadingStateChanged;
        }

        #endregion

        #region Private Methods

        private void DownloadHandler_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            EventHandler handler = OnDownloadUpdateComplete;
            handler?.Invoke(this, new EventArgs());
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                var frame = _browser.GetMainFrame();
                // Get Document Height
                var task = frame.EvaluateScriptAsync("(function() { return document.body.innerHTML; })();", null);

                task.ContinueWith(t =>
                {
                    if (!t.IsFaulted)
                    {
                        _browser.LoadingStateChanged -= Browser_LoadingStateChanged;

                        var response = t.Result;
                        string responseStr = (string)response.Result;
                        string containsStr = string.Format(LINK_INDEX_TEMPLATE, _symbol);

                        int startStrIndex = responseStr.IndexOf(containsStr);

                        if(startStrIndex < 0)
                        {
                            return;
                        }

                        int endStrIndex = responseStr.IndexOf('"', startStrIndex);

                        string newLink = responseStr.Substring(startStrIndex, endStrIndex - startStrIndex).Replace("&amp;", "&");
                        _browser.Load(newLink);
                    }
                }, TaskScheduler.Default);
            }
        }

        #endregion

        #region Private Data

        private readonly ChromiumWebBrowser _browser;
        private readonly DownloadHandler _downloadHandler;

        private readonly string _symbol;
        private readonly string _name;
        private readonly string _saveDirectory;

        private const string LINK_INDEX_TEMPLATE = "https://query1.finance.yahoo.com/v7/finance/download/{0}?period1";
        private const string URL_TEMPLATE = "https://finance.yahoo.com/quote/{0}/history?period1=852076800&period2=1924905600&interval=1d&filter=history&frequency=1d";

        #endregion
    }
}
