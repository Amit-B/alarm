using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
namespace Alarm
{
    public partial class UpdateVersion : Form
    {
        public UpdateVersion()
        {
            InitializeComponent();
            try
            {
                this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            }
            catch
            {
            }
        }
        private WebClient wc = new WebClient();
        private Loading dl = new Loading(true);
        private int cur = 0;
        private string[] dlfiles;
        private string newest = string.Empty;
        private void UpdateVersion_Load(object sender, EventArgs e)
        {
            dlfiles = App.INIGetKey(App.path + "/conf.tmp", "DownloadFiles").Split('|');
            LoadMyLanguage();
            newest = App.INIGetKey(App.path + "/conf.tmp", "NewestVersion");
            label3.Text = App.version;
            label5.Text = newest;
            button1.Text = button1.Text.Replace("X", newest);
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            dl.Show();
            dl.progressBar1.Minimum = 0;
            dl.progressBar1.Maximum = 100;
            dl.progressBar1.Value = 0;
            dl.label1.Text = Variables.text["update.progress"].ToString().Replace("X", 1.ToString()).Replace("Y", (dlfiles.Length + 1).ToString());
            wc.DownloadFileCompleted += new AsyncCompletedEventHandler(wc_DownloadFileCompleted);
            wc.DownloadProgressChanged += new DownloadProgressChangedEventHandler(wc_DownloadProgressChanged);
            wc.DownloadFileAsync(new Uri(App.INIGetKey(App.path + "/conf.tmp", "Download")), Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Alarm.rar");
        }
        private void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            dl.progressBar1.Value = e.ProgressPercentage;
        }
        private void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            dl.progressBar1.Value = 0;
            cur++;
            dl.label1.Text = Variables.text["update.progress"].ToString().Replace("X", (cur + 1).ToString()).Replace("Y", (dlfiles.Length + 1).ToString());
            if (cur == dlfiles.Length)
            {
                File.Delete(App.path + "/conf.tmp");
                File.WriteAllText(App.path + "/Changelog.txt", App.ReadFromWeb(App.programurl + "changelog/" + newest + ".txt"));
                dl.Hide();
                new UpdateComplete().ShowDialog();
                Application.Exit();
            }
            else wc.DownloadFileAsync(new Uri(App.programurl + "install/" + dlfiles[cur - 1]), App.path + "/" + dlfiles[cur - 1]);
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Text = Variables.text["update"].ToString();
            this.label1.Text = Variables.text["update.attention"].ToString();
            this.textBox1.Text = Variables.text["update.text"].ToString();
            this.label2.Text = Variables.text["update.your"].ToString();
            this.label4.Text = Variables.text["update.our"].ToString();
            this.button1.Text = Variables.text["update.dl"].ToString();
        }
    }
}