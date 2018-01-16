using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Diagnostics;
namespace Alarm
{
    public partial class Not : Form
    {
        private Main m;
        private Notification cur = null;
        public SoundPlayer[] sp = { null, null };
        public Not(Main m)
        {
            InitializeComponent();
            this.m = m;
        }
        private void Not_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            UpdateSoundPlayers();
            this.Location = new Point(
                Variables.setting["side"].ToString() == "right" ? (Screen.PrimaryScreen.Bounds.Width - this.Size.Width) : 0,
                Screen.PrimaryScreen.Bounds.Height - this.Size.Height - 30);
        }
        public void UpdateSoundPlayers()
        {
            this.sp[0] = Variables.setting["alarmfile"].ToString() != "null" && File.Exists(App.path + "/" + Variables.setting["alarmfile"]) && Variables.setting["sound"].ToString() == "1" ? new SoundPlayer(App.path + "/" + Variables.setting["alarmfile"]) : null;
            this.sp[1] = Variables.setting["soundfile"].ToString() != "null" && File.Exists(App.path + "/" + Variables.setting["soundfile"]) && Variables.setting["sound"].ToString() == "1" ? new SoundPlayer(App.path + "/" + Variables.setting["soundfile"]) : null;
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.label1.Text = Variables.text["not"].ToString();
            this.linkLabel1.Text = Variables.text["not.setting"].ToString();
            this.linkLabel2.Text = Variables.text["not.readmore"].ToString();
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m.AppState(true);
            m.Focus();
            Setting s = new Setting();
            s.Parent = m;
            s.ShowDialog();
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            m.fullnot.SetNot(cur);
            m.fullnot.Show();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.cur = null;
            this.Hide();
            if(int.Parse(Variables.setting["ntime"].ToString()) > 0) this.timer1.Stop();
        }
        public void ShowNotification(Notification n)
        {
            if (Variables.setting["st"].ToString() != "00:00" &&
                DateTime.Now.Hour < int.Parse(Variables.setting["st"].ToString().Split(':')[0]) &&
                DateTime.Now.Hour > int.Parse(Variables.setting["et"].ToString().Split(':')[0]) &&
                DateTime.Now.Minute < int.Parse(Variables.setting["st"].ToString().Split(':')[1]) &&
                DateTime.Now.Minute > int.Parse(Variables.setting["et"].ToString().Split(':')[1]))
                return;
            int num = App.NumOfNotType(n.type);
            if (Variables.setting["not" + num].ToString() == "1")
            {
                if (this.cur != null)
                {
                    if (int.Parse(Variables.setting["ntime"].ToString()) > 0) timer1.Stop();
                    this.Hide();
                }
                this.Show();
                this.cur = n;
                label2.Text = Variables.text["not.not" + num].ToString();
                textBox1.Text = cur.title;
                textBox1.SelectionLength = 0;
                if (cur.sitelink.Length > 0)
                {
                    textBox1.ForeColor = Color.Blue;
                    textBox1.Cursor = Cursors.Hand;
                    textBox1.Font = new System.Drawing.Font("Arial", 8.25F, FontStyle.Underline, GraphicsUnit.Point, ((byte)(177)));
                }
                else
                {
                    textBox1.ForeColor = Color.Black;
                    textBox1.Cursor = Cursors.IBeam;
                    textBox1.Font = new System.Drawing.Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(177)));
                }
                if (int.Parse(Variables.setting["ntime"].ToString()) > 0)
                {
                    timer1.Interval = int.Parse(Variables.setting["ntime"].ToString()) * 1000;
                    timer1.Start();
                }
                if (Variables.setting["sound"].ToString() == "1")
                    switch (num)
                    {
                        case 1: if (sp[1] != null) sp[1].Play(); break;
                        default: if (sp[0] != null) sp[0].Play(); break;
                    }
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1_Tick(sender, e);
        }
        private void textBox1_Click(object sender, EventArgs e)
        {
            if (cur.sitelink.Length > 0)
                App.OpenWebSite(cur.sitelink);
        }
    }
}