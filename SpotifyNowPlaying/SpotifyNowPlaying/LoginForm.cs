using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;

namespace SpotifyNowPlaying
{
    public partial class LoginForm : Form
    {
        public LoginForm(String uri)
        {
            InitializeComponent();

            InitializeChromium(uri);
        }

        public ChromiumWebBrowser chromeBrowser;

        public void InitializeChromium(String uri)
        {

            CefSettings settings = new CefSettings();
            settings.CachePath = "ChromiumCache/";

            if (!Cef.IsInitialized)
            {
                Cef.Initialize(settings);
            }
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(uri);

            chromeBrowser.LoadingStateChanged += this.ChromeTextChaned;

            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;
        }

        public void ChromeTextChaned(object sender, LoadingStateChangedEventArgs e)
        {
            if(!e.IsLoading)
            {
                var task = chromeBrowser.EvaluateScriptAsync("document.getElementsByTagName(\"pre\")[0] != undefined");

                task.ContinueWith(r2 =>
                {
                    if(r2.Result.Success)
                    {
                        var task2 = chromeBrowser.EvaluateScriptAsync("document.getElementsByTagName(\"pre\")[0].innerHTML == \"OK - This window can be closed now\"");

                        task2.ContinueWith(r =>
                        {
                            if (r.Result.Success)
                            {
                                try { 
                                this.Invoke((MethodInvoker)delegate
                                {
                                    this.Close();
                                });
                                } catch (System.ComponentModel.InvalidAsynchronousStateException)
                                {
                                }
                            }
                        });
                    }
                });
            }
        }
    }
}
