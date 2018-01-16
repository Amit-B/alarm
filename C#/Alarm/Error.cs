using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class Error : Form
    {
        public Error(string err)
        {
            InitializeComponent();
            textBox1.Text = err;
            try
            {
                this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            }
            catch
            {
            }
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        public void LoadMyLanguage()
        {
            try
            {
                this.Text = Variables.text["error"].ToString();
                this.label1.Text = Variables.text["error.title"].ToString();
                this.button1.Text = Variables.text["quit"].ToString();
            }
            catch
            {
            }
        }
        private void Error_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
        }
        private void Error_FormClosing(object sender, FormClosingEventArgs e)
        {
            button1_Click(sender, e);
        }
    }
}