using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace Alarm
{
    public partial class Changelog : Form
    {
        public Changelog()
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void Changelog_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            textBox1.Text = App.ReadK(File.ReadAllText(App.path + "/Changelog.txt"));
            File.Delete(App.path + "/Changelog.txt");
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["changelog"].ToString().Replace("X", App.version);
            this.label1.Text = Variables.text["changelog.list"].ToString().Replace("X", App.version);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Changelog_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageBox.Show(Variables.text["changelog.close"].ToString(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}