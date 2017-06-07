// Copyright © 2010-2017 The CefSharp Authors. All rights reserved.
//
// Use of this source code is governed by a BSD-style license that can be found in the LICENSE file.

using System;
using System.IO;

namespace CefSharp.Example
{
    public class DownloadHandler : IDownloadHandler
    {
        public event EventHandler<DownloadItem> OnBeforeDownloadFired;

        public event EventHandler<DownloadItem> OnDownloadUpdatedFired;

        public DownloadHandler(string downloadDirectory, string fileName)
        {
            _fileName = fileName;
            _directoryName = downloadDirectory;
        }

        public void OnBeforeDownload(IBrowser browser, DownloadItem downloadItem, IBeforeDownloadCallback callback)
        {
            var handler = OnBeforeDownloadFired;
            if (handler != null)
            {
                handler(this, downloadItem);
            }

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

        private readonly string _directoryName;
        private readonly string _fileName;
    }
}