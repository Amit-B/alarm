using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class Language : Form
    {
        Main main = null;
        public Language(Main main)
        {
            InitializeComponent();
            this.main = main;
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        private void Language_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            main.Focus();
            this.Close();
        }
        private void ChangeLanguage(string to)
        {
            this.Enabled = false;
            Loading load = new Loading(true);
            load.Show();
            load.progressBar1.Maximum = 7;
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "language", to);
            load.progressBar1.Value++;
            App.ReadConfiguration(App.path + "/" + to + ".lang", '=', ref Variables.text);
            load.progressBar1.Value++;
            this.LoadMyLanguage();
            load.progressBar1.Value++;
            main.LoadMyLanguage();
            load.progressBar1.Value++;
            main.not.LoadMyLanguage();
            load.progressBar1.Value++;
            main.fullnot.LoadMyLanguage();
            load.progressBar1.Value++;
            switch (to)
            {
                case "he": main.pictureBox3.Image = Images.Israel; break;
                case "en": main.pictureBox3.Image = Images.United_Kingdom; break;
                case "ar": main.pictureBox3.Image = Images.Palestine; break;
                case "ru": main.pictureBox3.Image = Images.Russia; break;
            }
            load.progressBar1.Value++;
            load.Close();
            this.Enabled = true;
            this.Focus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ChangeLanguage("he");
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ChangeLanguage("en");
        }
        private void button6_Click(object sender, EventArgs e)
        {
            ChangeLanguage("ar");
        }
        private void button5_Click(object sender, EventArgs e)
        {
            ChangeLanguage("ru");
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["lang"].ToString();
            this.button1.Text = Variables.text["close"].ToString();
        }
    }
}