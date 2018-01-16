using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace Alarm
{
    public partial class Loading : Form
    {
        public bool wt = false;
        public Loading(bool wt)
        {
            InitializeComponent();
            this.wt = wt;
        }
        private void Loading_Load(object sender, EventArgs e)
        {
            if(wt) LoadMyLanguage();
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Text = Variables.text["loading"].ToString();
            this.label1.Text = Variables.text["loading.text"].ToString();
            this.label1.Refresh();
            this.Show();
            this.Focus();
        }
    }
}