using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpotifyNowPlaying
{
    public partial class NowPlayingInterface : Form
    {
        private String fullName;
        private int namePos;
        private String nameDirection;
        private int nameStoppedTicks;
        
        private String fullArtists;
        private int artistPos;
        private String artistDirection;
        private int artistsStoppedTicks;

        private Options opt;

        private decimal wait = OptionManager.waitDelay;

        public NowPlayingInterface()
        {
            InitializeComponent();
            this.toolTip1.SetToolTip(this, "Right click for more options");
            this.timerName.Interval = Decimal.ToInt32(OptionManager.scrollSpeed);
            this.timerName.Tick += new EventHandler(this.NameTick);
            this.timerArtists.Interval = Decimal.ToInt32(OptionManager.scrollSpeed);
            this.timerArtists.Tick += new EventHandler(this.ArtistsTick);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.opt = null;
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0x84:
                    base.WndProc(ref m);
                    if ((int)m.Result == 0x1)
                        m.Result = (IntPtr)0x2;
                    return;
            }
            base.WndProc(ref m);
        }

        public void updateValues(decimal speed, decimal wait)
        {
            this.timerName.Interval = Decimal.ToInt32(speed);
            this.timerArtists.Interval = Decimal.ToInt32(speed);
            this.wait = wait;
        }

        public void updateInterface(string title, string artists, string imageURI)
        {
            if(title != this.fullName && artists != this.fullArtists) { 
                this.Invoke((MethodInvoker)delegate
                {
                    this.fullName = title;
                    if(this.fullName.Length > 20)
                    {
                        this.label1.Text = this.fullName.Substring(0, 20);
                        this.namePos = 0;
                        this.nameStoppedTicks = 0;
                        this.nameDirection = "r";
                        if (!this.timerName.Enabled)
                        {
                            this.timerName.Start();
                        }
                    } else
                    {
                        this.label1.Text = this.fullName;
                        if(this.timerName.Enabled)
                        {
                            this.timerName.Stop();
                        }
                    }
                    this.fullArtists = artists;
                    if(this.fullArtists.Length > 30)
                    {
                        this.label2.Text = this.fullArtists.Substring(0, 30);
                        this.artistPos = 0;
                        this.artistsStoppedTicks = 0;
                        this.artistDirection = "r";
                        if(!this.timerArtists.Enabled)
                        {
                            this.timerArtists.Start();
                        }
                    } else
                    {
                        this.label2.Text = this.fullArtists;
                        if(this.timerArtists.Enabled)
                        {
                            this.timerArtists.Stop();
                        }
                    }
                    this.pictureBox1.Load(imageURI);
                });
            }
        }

        private void NameTick(object sender, EventArgs e)
        {
            if(this.nameDirection == "r") {
                if(this.fullName.Length-this.namePos > 20) { 
                    this.namePos++;
                    this.label1.Text = this.fullName.Substring(this.namePos, 20);
                } else
                {
                    this.nameStoppedTicks++;
                    if(this.nameStoppedTicks == wait)
                    {
                        this.nameDirection = "l";
                        this.nameStoppedTicks = 0;
                    }
                }
            } else
            {
                if (this.namePos > 0)
                {
                    this.namePos--;
                    this.label1.Text = this.fullName.Substring(this.namePos, 20);
                } else
                {
                    this.nameStoppedTicks++;
                    if(this.nameStoppedTicks == wait)
                    {
                        this.nameStoppedTicks = 0;
                        this.nameDirection = "r";
                    }
                }
            }
        }

        private void ArtistsTick(object sender, EventArgs e)
        {
            if(this.artistDirection == "r")
            {
                if(this.fullArtists.Length-this.artistPos > 30)
                {
                    this.artistPos++;
                    this.label2.Text = this.fullArtists.Substring(this.artistPos, 30);
                } else
                {
                    this.artistsStoppedTicks++;
                    if(this.artistsStoppedTicks == wait)
                    {
                        this.artistDirection = "l";
                        this.artistsStoppedTicks = 0;
                    }
                }
            } else
            {
                if (this.artistPos > 0)
                {
                    this.artistPos--;
                    this.label2.Text = this.fullArtists.Substring(this.artistPos, 30);
                }
                else
                {
                    this.artistsStoppedTicks++;
                    if (this.artistsStoppedTicks == wait)
                    {
                        this.artistDirection = "r";
                        this.artistsStoppedTicks = 0;
                    }
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.opt == null || this.opt.IsDisposed)
            {
                this.opt = new Options();
            }
            this.opt.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("SpotifyNowPlaying v1.0.1\nBy c0derMo");
        }

        private void bugsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/c0derMo/SpotifyNowPlaying/issues");
        }
    }
}
