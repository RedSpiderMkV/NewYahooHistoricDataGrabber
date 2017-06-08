using System;
using System.Windows.Forms;
using CefSharp;

namespace YahooFinanceHistoricData
{
    public partial class BrowserHost : Form
    {
        private readonly YahooBrowser _yahooBrowser;

        public BrowserHost(YahooBrowser yahooBrowser)
        {
            _yahooBrowser = yahooBrowser;

            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            _yahooBrowser.OnDownloadUpdateComplete += _yahooBrowser_OnDownloadUpdateComplete;
            panel1.Controls.Add(_yahooBrowser.Browser);

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;

            timer.Interval = 45000;
            timer.Start();
        }

        private void _yahooBrowser_OnDownloadUpdateComplete(object sender, EventArgs e)
        {
            Console.WriteLine("APPLICATION: Download OK.");
            
            if(this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Close()));
            }
            else
            {
                this.Close();
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("APPLICATION: Time out...");
            
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => this.Close()));
            }
            else
            {
                this.Close();
            }
        }
    }
}
