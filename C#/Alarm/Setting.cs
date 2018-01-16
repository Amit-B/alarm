using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Media;
namespace Alarm
{
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void Setting_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            CheckBox[] cb = { checkBox1, checkBox4 };
            string[] sett = { "sound", "startup" };
            for(int i = 0; i < cb.Length; i++)
                cb[i].Checked = Convert.ToInt32(Variables.setting[sett[i]].ToString()) == 1;
            LoadFiles();
            numericUpDown1.Value = decimal.Parse(Variables.setting["ntime"].ToString());
            maskedTextBox1.Text = Variables.setting["st"].ToString();
            maskedTextBox2.Text = Variables.setting["et"].ToString();
            if (Variables.setting["side"].ToString() == "right") radioButton1.Checked = true;
            else radioButton2.Checked = false;
        }
        private void LoadFiles()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            string[] sounds = Directory.GetFiles(App.path, "*.wav");
            FileInfo fi;
            int c1 = -1, c2 = -1;
            for (int i = 0; i < sounds.GetLength(0); i++)
            {
                fi = new FileInfo(sounds[i]);
                comboBox1.Items.Add(fi.Name);
                comboBox2.Items.Add(fi.Name);
                if (Variables.setting["alarmfile"].ToString() == fi.Name)
                    c1 = i;
                if (Variables.setting["soundfile"].ToString() == fi.Name)
                    c2 = i;
            }
            comboBox1.Items.Add(Variables.text["setting.nosound"]);
            comboBox2.Items.Add(Variables.text["setting.nosound"]);
            comboBox1.SelectedIndex = c1 == -1 ? comboBox1.Items.Count - 1 : c1;
            comboBox2.SelectedIndex = c2 == -1 ? comboBox2.Items.Count - 1 : c2;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.facebook.com/MivzakLive");
        }
        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://twitter.com/mivzaklive");
        }
        private void toolStripStatusLabel5_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.youtube.com/user/mivzaklive");
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "sound", (checkBox1.Checked ? 1 : 0).ToString());
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "startup", (checkBox4.Checked ? 1 : 0).ToString());
            string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "/Alarm.exe";
            if(File.Exists(startup)) File.Delete(startup);
            if (checkBox4.Checked) File.Copy(Application.ExecutablePath, startup, true);
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == comboBox1.Items.Count - 1)
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "alarmfile", "null");
            else if (File.Exists(App.path + "/" + comboBox1.Items[comboBox1.SelectedIndex].ToString()))
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "alarmfile", comboBox1.Items[comboBox1.SelectedIndex].ToString());
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == comboBox2.Items.Count - 1)
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "soundfile", "null");
            else if (File.Exists(App.path + "/" + comboBox2.Items[comboBox2.SelectedIndex].ToString()))
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "soundfile", comboBox2.Items[comboBox2.SelectedIndex].ToString());
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["setting"].ToString();
            this.checkBox1.Text = Variables.text["setting.sound"].ToString();
            this.checkBox4.Text = Variables.text["setting.startup"].ToString();
            this.label3.Text = Variables.text["setting.choosealarm"].ToString();
            this.label5.Text = Variables.text["setting.choosesound"].ToString();
            this.comboBox1.Text = Variables.text["setting.listdef"].ToString();
            this.comboBox2.Text = Variables.text["setting.listdef"].ToString();
            this.label1.Text = Variables.text["setting.auto"].ToString();
            this.button2.Text = Variables.text["setting.anytime"].ToString();
            this.button3.Text = Variables.text["setting.donthide"].ToString();
            this.button4.Text = Variables.text["setting.addsound"].ToString();
            this.label2.Text = Variables.text["setting.to"].ToString();
            this.label4.Text = Variables.text["setting.exp"].ToString();
            this.label6.Text = Variables.text["setting.autohide"].ToString();
            this.label7.Text = Variables.text["setting.seconds"].ToString();
            this.label8.Text = Variables.text["setting.side"].ToString();
            this.radioButton1.Text = Variables.text["setting.right"].ToString();
            this.radioButton2.Text = Variables.text["setting.left"].ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            maskedTextBox1.Text = "00:00";
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "st", "00:00");
            maskedTextBox2.Text = "00:00";
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "et", "00:00");
        }
        private void Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "st", maskedTextBox1.Text);
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "et", maskedTextBox2.Text);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            numericUpDown1.Value = 0;
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "ntime", 0.ToString());
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "ntime", numericUpDown1.Value.ToString());
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = Variables.text["setting.addsound"].ToString();
            ofd.Filter = "Wav Files|*.wav";
            ofd.SupportMultiDottedExtensions = false;
            ofd.Multiselect = false;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(ofd.FileName);
                for(int i = 0; i < comboBox1.Items.Count - 1; i++)
                    if (comboBox1.Items[i].ToString() == fi.Name)
                    {
                        MessageBox.Show(Variables.text["setting.addfailed"].ToString(), Variables.text["setting.addsound"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                File.Copy(fi.FullName, App.path + "/" + fi.Name);
                LoadFiles();
                MessageBox.Show(Variables.text["setting.addsuccess"].ToString(), Variables.text["setting.addsound"].ToString(), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1 || comboBox1.SelectedIndex == comboBox1.Items.Count - 1 || comboBox1.Items.Count == 0) return;
            SoundPlayer sp = new SoundPlayer(App.path + "/" + comboBox1.Items[comboBox1.SelectedIndex].ToString());
            sp.Play();
            sp.Dispose();
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == -1 || comboBox2.SelectedIndex == comboBox2.Items.Count - 1 || comboBox2.Items.Count == 0) return;
            SoundPlayer sp = new SoundPlayer(App.path + "/" + comboBox2.Items[comboBox2.SelectedIndex].ToString());
            sp.Play();
            sp.Dispose();
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "side", "right");
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "side", "left");
        }
    }
}