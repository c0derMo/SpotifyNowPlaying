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
        public NowPlayingInterface()
        {
            InitializeComponent();
        }

        public void updateInterface(string title, string artists, string imageURI)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.label1.Text = title;
                this.label2.Text = artists;
                this.pictureBox1.Load("https://i.scdn.co/image/255621d727d0cf961a422059052965fae600c803");
            });
        }
    }
}
