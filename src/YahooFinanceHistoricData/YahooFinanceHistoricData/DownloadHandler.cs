using System;
using System.IO;
using CefSharp;

namespace YahooFinanceHistoricData
{
    public class DownloadHandler : IDownloadHandler
    {
        #region Events

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        #endregion

        #region Public Methods

        public DownloadHandler(string downloadDirectory, string fileName)
        {
            _fileName = fileName;
            _directoryName = downloadDirectory;
        }

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            if (!callback.IsDisposed)
            {
                using (callback)
                {
                    callback.Continue(Path.Combine(_directoryName, _fileName), false);
                }
            }
        }

        public void OnDownloadUpdated(IBrowser browser, DownloadItem downloadItem, IDownloadItemCallback callback)
        {
            if(downloadItem.IsComplete)
            {
                EventHandler<DownloadItem> handler = OnDownloadUpdatedFired;
                handler?.Invoke(this, downloadItem);
            }
        }

        #endregion

        #region Private Data

        private readonly string _directoryName;
        private readonly string _fileName;

        #endregion
    }
}