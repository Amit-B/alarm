using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
namespace Alarm
{
    public partial class LastNots : Form
    {
        private Main m;
        private const Notification.NotificationTypes TYPE = Notification.NotificationTypes.Alarm;
        private List<XmlNode> lastItems = null;
        public LastNots(Main m)
        {
            InitializeComponent();
            this.m = m;
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
        }
        private void LastNots_Load(object sender, EventArgs e)
        {
            lastItems = m.rss.GetLastItems(TYPE, m.rss.Count());
            numericUpDown1.Minimum = 0;
            numericUpDown1.Maximum = lastItems.Count;
            numericUpDown1.Value = numericUpDown1.Maximum;
            LoadMyLanguage();
            label1.Text = label1.Text.Replace("0",numericUpDown1.Maximum.ToString());
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Notification not = null;
            lastItems = m.rss.GetLastItems(TYPE, m.rss.Count());
            dataGridView1.Rows.Clear();
            for (int i = 0; i < numericUpDown1.Value; i++)
            {
                not = RSS.ConvertToNotification(lastItems[i]);
                dataGridView1.Rows.Add(new object[]
                {
                    (i+1).ToString(),
                    not.title,
                    not.time
                });
            }
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
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Text = Variables.text["lastnots"].ToString();
            this.dataGridView1.Columns[0].HeaderText = Variables.text["lastnots.n"].ToString();
            this.dataGridView1.Columns[1].HeaderText = Variables.text["lastnots.i"].ToString();
            this.dataGridView1.Columns[2].HeaderText = Variables.text["lastnots.d"].ToString();
            this.label1.Text = Variables.text["lastnots.max"].ToString();
            this.label2.Text = Variables.text["lastnots.tip"].ToString();
        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > dataGridView1.Rows.Count) return;
            m.fullnot.SetNot(RSS.ConvertToNotification(lastItems[e.RowIndex]));
            dataGridView1.ClearSelection();
            m.fullnot.Show();
        }
    }
}