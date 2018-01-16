using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class More : Form
    {
        private Main m = null;
        public More(Main m)
        {
            InitializeComponent();
            this.m = m;
        }
        private void More_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["more"].ToString();
            this.button1.Text = Variables.text["close"].ToString();
            this.button2.Text = Variables.text["more.lastnots"].ToString();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            new LastNots(this.m).ShowDialog();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}