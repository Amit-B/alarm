using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class VersionInfo : Form
    {
        public VersionInfo(int x, int y)
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Location = new Point(x, y);
        }
        private void VersionInfo_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            label2.Text = App.version;
            ViewChangelog(App.version);
        }
        public void ViewChangelog(string ver)
        {
            linkLabel1.Text = linkLabel1.Text.Replace("X", ver);
            textBox1.ResetText();
            textBox1.Refresh();
            label3.Visible = true;
            label3.Refresh();
            try
            {
                textBox1.Text = App.ReadK(App.ReadFromWeb(App.programurl + "changelog/" + ver + ".txt"));
                textBox1.Refresh();
            }
            catch
            {
            }
            label3.Visible = false;
            label3.Refresh();
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["version"].ToString().Replace("X", App.version);
            this.label1.Text = Variables.text["version.current"].ToString();
            this.label3.Text = Variables.text["loading.text"].ToString();
            this.linkLabel1.Text = Variables.text["version.changelog"].ToString();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SelectVersion sv = new SelectVersion();
            sv.ShowDialog();
            if (sv.choosen.Length > 0)
                ViewChangelog(sv.choosen);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}