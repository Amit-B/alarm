using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        private void About_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            button5.Text = button5.Text.Replace("X", App.version);
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["about"].ToString();
            this.button1.Text = Variables.text["about.facebook"].ToString();
            this.button2.Text = Variables.text["about.site"].ToString();
            this.button3.Text = Variables.text["about.twitter"].ToString();
            this.button4.Text = Variables.text["about.youtube"].ToString();
            this.button5.Text = Variables.text["version"].ToString();
            this.textBox2.Text = Variables.setting["language"].ToString() == "he" ?
@"התוכנה פותחה על ידי עמית בראמי
amitbarami@hotmail.com
כל הזכויות שמורות © 2011-2012" :
@"Developed by Amit Barami
amitbarami@hotmail.com
Copyright © 2011-2012";
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.facebook.com/MivzakLive");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://twitter.com/mivzaklive");
        }
        private void button4_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.youtube.com/user/mivzaklive");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            new VersionInfo(this.Location.X, this.Location.Y).ShowDialog();
        }
    }
}