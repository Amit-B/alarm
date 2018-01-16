using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class FullNot : Form
    {
        public FullNot()
        {
            InitializeComponent();
        }
        string link = string.Empty, sitelink = string.Empty;
        private void FullNot_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(link);
        }
        public void SetNot(Notification n)
        {
            int num = App.NumOfNotType(n.type);
            label2.Text = Variables.text["not.not" + num].ToString();
            string title = n.title;
            const int MAX_CHARS = 34;
            if (title.Length >= MAX_CHARS) title = title.Remove(MAX_CHARS - 3, title.Length - (MAX_CHARS - 3)) + "...";
            this.Text = title;
            textBox2.Text = n.time;
            textBox1.Text = n.description;
            link = n.link;
            button3.Visible = n.sitelink.Length > 0;
            sitelink = n.sitelink;
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.label1.Text = Variables.text["not.title"].ToString();
            this.button1.Text = Variables.text["close"].ToString();
            this.button2.Text = Variables.text["not.readmore"].ToString();
        }
        private void FullNot_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(sitelink);
        }
    }
}