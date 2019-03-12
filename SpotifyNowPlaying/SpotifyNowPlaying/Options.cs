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
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();

            this.numericUpDown3.Value = OptionManager.checkInterval;
            this.numericUpDown2.Value = OptionManager.scrollSpeed;
            this.numericUpDown1.Value = OptionManager.waitDelay;
            this.checkBox1.Checked = OptionManager.outputToTextFile;
            this.textBox1.Text = OptionManager.nameFile;
            this.textBox2.Text = OptionManager.artistsFile;
            this.textBox3.Text = OptionManager.imageFile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Program.startAuthentificationWindow(Program.auth);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CefSharp.OffScreen.ChromiumWebBrowser browser = new CefSharp.OffScreen.ChromiumWebBrowser("https://spotify.com/us/logout");
            Program.logout = true;
            System.Windows.Forms.MessageBox.Show("Logout sucessful!");
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            OptionManager.setCheckInterval(this.numericUpDown3.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            OptionManager.setScrollSpeed(this.numericUpDown2.Value);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            OptionManager.setWaitDelay(this.numericUpDown1.Value);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            OptionManager.setOutput(this.checkBox1.Checked);
            if(this.checkBox1.Checked)
            {
                this.textBox1.Enabled = true;
                this.textBox2.Enabled = true;
                this.textBox3.Enabled = true;
            } else
            {
                this.textBox1.Enabled = false;
                this.textBox2.Enabled = false;
                this.textBox3.Enabled = false;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            OptionManager.setNameFile(textBox1.Text);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            OptionManager.setArtistFile(textBox2.Text);
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            OptionManager.setImageFile(textBox3.Text);
        }

    }
}
