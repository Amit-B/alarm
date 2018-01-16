using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class SelectVersion : Form
    {
        public SelectVersion()
        {
            InitializeComponent();
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        public string choosen = string.Empty;
        private string[] list;
        private int sel = 0;
        private void SelectVersion_Load(object sender, EventArgs e)
        {
            LoadMyLanguage();
            list = App.ReadFromWeb(App.programurl + "/changelog/list.cfg").Split('\n');
            for (int i = 0; i < list.Length; i++)
                comboBox1.Items.Add(list[i]);
            comboBox1.SelectedIndex = 0;
        }
        public void LoadMyLanguage()
        {
            this.Text = Variables.text["version.select"].ToString();
            this.button1.Text = Variables.text["version.select2"].ToString();
            this.button2.Text = Variables.text["back"].ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            choosen = list[sel];
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            choosen = string.Empty;
            this.Close();
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text != list[sel])
            {
                comboBox1.SelectedIndex = -1;
                comboBox1.SelectedIndex = sel;
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sel = comboBox1.SelectedIndex;
        }
    }
}