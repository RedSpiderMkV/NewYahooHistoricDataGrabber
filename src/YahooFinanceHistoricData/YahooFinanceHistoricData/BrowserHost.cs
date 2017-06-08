using System;
using System.Windows.Forms;

namespace YahooFinanceHistoricData
{
    public partial class BrowserHost : Form
    {
        public int ReturnCode { get; private set; }

        public BrowserHost(YahooBrowser yahooBrowser)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            yahooBrowser.OnDownloadUpdateComplete += _yahooBrowser_OnDownloadUpdateComplete;
            panel1.Controls.Add(yahooBrowser.Browser);

            Timer timer = new Timer();
            timer.Tick += Timer_Tick;

            timer.Interval = 30000;
            timer.Start();
        }

        private void _yahooBrowser_OnDownloadUpdateComplete(object sender, EventArgs e)
        {
            ReturnCode = 0;
            CloseForm("APPLICATION: Download OK.");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ReturnCode = 1;
            CloseForm("APPLICATION: Time out...");
        }

        private void CloseForm(string message)
        {
            Console.WriteLine(message);
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
