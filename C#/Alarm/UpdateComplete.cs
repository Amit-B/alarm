using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class UpdateComplete : Form
    {
        public UpdateComplete()
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
        private void UpdateComplete_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Text = Variables.text["update"].ToString();
            this.label1.Text = Variables.text["update.complete"].ToString();
            this.button1.Text = Variables.text["update.start"].ToString();
            this.button2.Text = Variables.text["quit"].ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/Alarm.rar");
            this.Close();
        }
    }
}