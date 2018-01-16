using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class Mail : Form
    {
        public Mail()
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        const int MAX_CHARS = 500;
        private void Mail_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            textBox1.Text = App.mail;
            textBox3.MaxLength = MAX_CHARS;
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["mail"].ToString();
            this.label1.Text = Variables.text["mail.to"].ToString();
            this.label2.Text = Variables.text["mail.from"].ToString();
            this.label7.Text = Variables.text["mail.frommail"].ToString();
            this.label3.Text = Variables.text["mail.subject"].ToString();
            this.label4.Text = Variables.text["mail.phone"].ToString();
            this.label5.Text = Variables.text["mail.body"].ToString();
            this.label6.Text = Variables.text["mail.count"].ToString().Replace("X", 0.ToString()).Replace("Y", MAX_CHARS.ToString());
            this.button1.Text = Variables.text["close"].ToString();
            this.button2.Text = Variables.text["mail.send"].ToString();
            for (int i = 1; i <= 2; i++)
                this.comboBox1.Items.Add(Variables.text["mail.subj" + i].ToString());
            this.comboBox1.Items.Add(Variables.text["mail.other"].ToString());
            this.comboBox1.Text = Variables.text["mail.default"].ToString();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (!App.IsValidEmail(textBox6.Text)) MessageBox.Show(Variables.text["mail.failed1"].ToString(), "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (comboBox1.SelectedIndex == -1 || (comboBox1.SelectedIndex == comboBox1.Items.Count - 1 && textBox5.Text.Length == 0)) MessageBox.Show(Variables.text["mail.failed2"].ToString(), "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox3.Text.Length < 10) MessageBox.Show(Variables.text["mail.failed3"].ToString(), "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (textBox2.Text.Length <= 1) MessageBox.Show(Variables.text["mail.failed4"].ToString(), "Mail", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                string body = string.Empty;
                body += "Name: " + textBox2.Text + "\r\n";
                body += "Mail: " + textBox6.Text + "\r\n";
                body += "Phone: " + textBox4.Text + "\r\n";
                body += "Subject: " + (comboBox1.SelectedIndex == comboBox1.Items.Count - 1 ? textBox5.Text : Variables.text["mail.subj" + (comboBox1.SelectedIndex + 1)].ToString()) + "\r\n\r\n\r\n";
                body += textBox3.Text + "\r\n\r\n\r\n- סוף ההודעה -";
                bool s = App.Mail(App.mail, "MivzakLive Alarm Contact: " + (comboBox1.SelectedIndex == comboBox1.Items.Count - 1 ? textBox5.Text : Variables.text["mail.subj" + (comboBox1.SelectedIndex + 1)].ToString()), body);
                MessageBox.Show(Variables.text["mail." + (s ? "success" : "failed5")].ToString(), "Mail", MessageBoxButtons.OK, s ? MessageBoxIcon.Information : MessageBoxIcon.Error);
                this.Close();
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox5.Visible = comboBox1.SelectedIndex == comboBox1.Items.Count - 1;
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label6.Text = Variables.text["mail.count"].ToString().Replace("X", textBox3.Text.Length.ToString()).Replace("Y", MAX_CHARS.ToString());
        }
    }
}