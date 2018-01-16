using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Net;
using System.Diagnostics;
using Microsoft.Win32;
namespace Alarm
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }
        public RSS rss = null;
        public Not not = null;
        public FullNot fullnot = null;
        public const int MAX_UPDATES = 15;
        public List<XmlNode> mivz = new List<XmlNode>(MAX_UPDATES);
        public Notification[] nots = new Notification[MAX_UPDATES];
        public WebClient client = null;
        public int ads = 0;
        public Timer startTimer = null;
        private void Main_Load(object sender, EventArgs e)
        {
            if (Application.ExecutablePath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Startup)))
            {
                startTimer = new Timer();
                startTimer.Interval = 2000;
                startTimer.Tick += new EventHandler(LoadApp);
                startTimer.Start();
            }
            else LoadApp(sender, e);
        }
        private void LoadApp(object sender, EventArgs e)
        {
            if (startTimer != null) startTimer.Stop();
            int code = 0;
            try
            {
                this.Enabled = false;
                bool firstconnect = false;
            tryingToCreate:
                {
                    try
                    {
                        if (!Directory.Exists(App.path))
                        {
                            Directory.CreateDirectory(App.path);
                            firstconnect = true;
                        }
                    }
                    catch
                    {
                        if (App.path == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/mivzaklive")
                        {
                            new Error("לתוכנה חסרות הרשאות לתיקיות Program Files / My Documents. נא להוסיף הרשאות או להפעיל כמנהל.").ShowDialog();
                            return;
                        }
                        else
                        {
                            App.path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/mivzaklive";
                            goto tryingToCreate;
                        }
                    }
                }
                code++;
                RegistryKey reg = Registry.CurrentUser.CreateSubKey("MivzakLive");
                if (firstconnect) reg.SetValue("Dir", App.path);
                else App.path = reg.GetValue("Dir", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "/mivzaklive").ToString();
                reg.Close();
                code++;
                if (Process.GetProcessesByName("Alarm").Length > 1)
                {
                    new Error("התוכנה כבר פועלת.").ShowDialog();
                    return;
                }
                Loading load = new Loading(false);
                load.Show();
                load.progressBar1.Minimum = 0;
                load.progressBar1.Maximum = 12;
                load.progressBar1.Value = 0;
                load.label1.Refresh();
                load.progressBar1.Refresh();
                if (!App.IsConnectedToInternet())
                {
                    new Error("לשימוש בתוכנה זו עליך להיות מחובר לאינטרנט.").ShowDialog();
                    return;
                }
                load.progressBar1.Value++;
                code++;
                File.WriteAllText(App.path + "/conf.tmp", App.ReadFromWeb(App.programurl + "configuration.cfg"));
                load.progressBar1.Value++;
                code++;
                if (App.INIGetKey(App.path + "/conf.tmp", "Lock") == "1")
                {
                    new Error(App.ReadFromWeb(App.INIGetKey(App.path + "/conf.tmp", "LockMessage"))).ShowDialog();
                    return;
                }
                client = new WebClient();
                load.progressBar1.Value++;
                code++;
                if (firstconnect)
                {
                    Directory.CreateDirectory(App.path);
                    for (int i = 0; i < App.files.Length; i++) client.DownloadFile(App.programurl + "install/" + App.files[i], App.path + "/" + App.files[i]);
                }
                else
                {
                    bool testone = App.CheckFiles(1);
                    if (!testone)
                    {
                        new Error("קבצי התוכנה פגומים, הורד אותה מחדש (Error #1).").ShowDialog();
                        return;
                    }
                }
                load.progressBar1.Value++;
                code++;
                App.ReadConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting);
                load.progressBar1.Value++;
                code++;
                App.ReadConfiguration(App.path + "/" + Variables.setting["language"] + ".lang", '=', ref Variables.text);
                load.progressBar1.Value++;
                code++;
                if (!App.CheckFiles(2)) new Error("קבצי התוכנה פגומים, הורד אותה מחדש (Error #2).");
                load.progressBar1.Value++;
                code++;
                if (App.INIGetKey(App.path + "/conf.tmp", "NewestVersion") != App.version)
                {
                    load.Hide();
                    new UpdateVersion().ShowDialog();
                    return;
                }
                load.progressBar1.Value++;
                code++;
                string u = App.INIGetKey(App.path + "/conf.tmp", "Footer");
                if (u.EndsWith(".html"))
                {
                    webBrowser1.Visible = true;
                    pictureBox4.Visible = false;
                    webBrowser1.Url = new Uri(u);
                }
                else
                {
                    webBrowser1.Visible = false;
                    pictureBox4.Visible = true;
                    client.DownloadFileAsync(new Uri(u), App.path + "/footer.jpg");
                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                }
                load.progressBar1.Value++;
                code++;
                App.website = App.INIGetKey(App.path + "/conf.tmp", "Website");
                App.mail = App.INIGetKey(App.path + "/conf.tmp", "Mail");
                File.Delete(App.path + "/conf.tmp");
                load.progressBar1.Value++;
                code++;
                rss = new RSS(App.rssurl, true);
                mivz = rss.GetLastItems(Notification.NotificationTypes.Any, MAX_UPDATES);
                Updates();
                load.progressBar1.Value++;
                code++;
                notifyIcon1.Visible = true;
                toolStripStatusLabel2.Text = "V " + App.version;
                toolStripStatusLabel6.Text = App.mail;
                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    checkedListBox1.SetItemChecked(i, Convert.ToInt32(Variables.setting["not" + (i + 1)].ToString()) == 1);
                not = new Not(this);
                fullnot = new FullNot();
                load.progressBar1.Value++;
                code++;
                LoadMyLanguage();
                code++;
                timer1.Start();
                load.Close();
                dataGridView1.ClearSelection();
                this.Enabled = true;
                this.Focus();
                if (firstconnect)
                {
                    ToolTip tt = new ToolTip();
                    tt.IsBalloon = true;
                    tt.ToolTipIcon = ToolTipIcon.Info;
                    tt.Show("לשינוי שפה לחץ על כאן\nClick here to switch language", this, 0, 0, 5000);
                    //File.Move(Application.ExecutablePath, App.path + "/Alarm.exe");
                }
                if (File.Exists(App.path + "/Changelog.txt"))
                {
                    new Changelog().ShowDialog();
                    string startup = Environment.GetFolderPath(Environment.SpecialFolder.Startup) + "/Alarm.exe";
                    if(File.Exists(startup)) File.Copy(Application.ExecutablePath, startup, true);
                }
                code++;
            }
            catch
            {
                if (File.Exists(App.path + "/conf.tmp")) File.Delete(App.path + "/conf.tmp");
                new Error("נוצרה בעיית טעינה, אנא נסה שוב. אם הבעיה חוזרת על עצמה צור קשר במייל: " + App.mail + "\r\n(קוד בעיה: " + code + ")").ShowDialog();
            }
        }
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                pictureBox4.Image = Image.FromFile(App.path + "/footer.jpg");
                client.Dispose();
            }
            catch
            {
            }
        }
        private void Updates()
        {
            dataGridView1.Rows.Clear();
            for (int i = 0; i < MAX_UPDATES; i++)
            {
                nots[i] = RSS.ConvertToNotification(mivz[i]);
                dataGridView1.Rows.Add(new object[]
                {
                    nots[i].title
                });
            }
            dataGridView1.ScrollBars = ScrollBars.Both;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AppState(false);
        }
        private void button5_Click(object sender, EventArgs e)
        {
            new Setting().ShowDialog();
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            AppState(!this.Visible);
        }
        private void הצגToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppState(!this.Visible);
        }
        public void AppState(bool vis)
        {
            if (vis)
            {
                this.Show();
                הצגToolStripMenuItem.Text = Variables.text["notify.hide"].ToString();
            }
            else
            {
                this.Hide();
                הצגToolStripMenuItem.Text = Variables.text["notify.show"].ToString();
            }
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            App.OpenWebSite(App.website);
        }
        private void button9_Click(object sender, EventArgs e)
        {
            new More(this).ShowDialog();
        }
        private void button8_Click(object sender, EventArgs e)
        {
            new Mail().ShowDialog();
        }
        private void toolStripStatusLabel8_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.2net.co.il/");
        }
        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("mailto:" + App.mail);
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (File.Exists(App.path + "/conf.tmp")) File.Delete(App.path + "/conf.tmp");
        }
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            new Language(this).ShowDialog();
        }
        public void LoadMyLanguage()
        {
            this.RightToLeft = Variables.text["rtl"].ToString() == "1" ? RightToLeft.Yes : RightToLeft.No;
            this.Text = Variables.text["main"].ToString();
            this.button1.Text = Variables.text["quit"].ToString();
            //this.button2.Text = Variables.text["main.minimize"].ToString(); - replaced with "-"
            this.button5.Text = Variables.text["main.setting"].ToString();
            this.button7.Text = Variables.text["main.about"].ToString();
            this.button8.Text = Variables.text["main.contact"].ToString();
            this.button9.Text = Variables.text["main.more"].ToString();
            this.groupBox1.Text = Variables.text["main.grp1"].ToString();
            this.groupBox2.Text = Variables.text["main.grp2"].ToString();
            this.groupBox3.Text = Variables.text["main.grp3"].ToString();
            this.button3.Text = Variables.text["main.markall"].ToString();
            this.button4.Text = Variables.text["main.markrem"].ToString();
            this.toolStripStatusLabel1.Text = Variables.text["main.footer"].ToString();
            this.toolStripStatusLabel8.Text = Variables.text["main.by"].ToString();
            this.הצגToolStripMenuItem.Text = Variables.text["notify.hide"].ToString();
            this.יציאהToolStripMenuItem.Text = Variables.text["quit"].ToString();
            this.notifyIcon1.Text = Variables.text["notify"].ToString();
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.Items[i] = Variables.text["main.not" + (i + 1)];
            switch (Variables.setting["language"].ToString())
            {
                case "he": this.pictureBox3.Image = Images.Israel; break;
                case "en": this.pictureBox3.Image = Images.United_Kingdom; break;
                case "ar": this.pictureBox3.Image = Images.Palestine; break;
                case "ru": this.pictureBox3.Image = Images.Russia; break;
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, true);
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "not" + (i + 1).ToString(), 1.ToString());
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
                App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "not" + (i + 1).ToString(), 0.ToString());
            }
        }
        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, "not" + (e.Index+1).ToString(), (e.NewValue == CheckState.Checked ? 1 : 0).ToString());
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            int way = 0;
            try
            {
                way++;
                XmlNode[] oldmivz = new XmlNode[MAX_UPDATES];
                way++;
                mivz.CopyTo(oldmivz);
                way++;
                rss.Refresh();
                way++;
                mivz = rss.GetLastItems(Notification.NotificationTypes.Any, MAX_UPDATES);
                way++;
                Notification n = RSS.ConvertToNotification(mivz[0]);
                way++;
                if (RSS.ConvertToNotification(oldmivz[0]).title != n.title)
                {
                    way = 10;
                    bool flag = false;
                    for (int i = 0; i < 5 && !flag; i++)
                        if (RSS.ConvertToNotification(oldmivz[i]).description == n.description)
                            flag = true;
                    way++;
                    if (!flag)
                    {
                        way = 15;
                        not.ShowNotification(n);
                        way++;
                        mivz = rss.GetLastItems(Notification.NotificationTypes.Any, MAX_UPDATES);
                        way++;
                        Updates();
                        way++;
                    }
                }
                ads++;
                if (ads == 3)
                {
                    //webBrowser1.Refresh();
                    ads = 0;
                }
            }
            catch
            {
                new Error("קיימת בעיית תוכנה כללית, אם הבעיה מתמשכת פנה אלינו במייל: " + App.mail + "\r\n(קוד בעיה: " + way + ")").ShowDialog();
            }
        }
        private void pictureBox4_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.facebook.com/MivzakLive");
        }
        private void button7_Click(object sender, EventArgs e)
        {
            new About().ShowDialog();
        }
        private void יציאהToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            fullnot.SetNot(nots[e.RowIndex]);
            dataGridView1.ClearSelection();
            fullnot.Show();
        }
        private void Main_Activated(object sender, EventArgs e)
        {
            dataGridView1.ClearSelection();
        }
        private void toolStripStatusLabel9_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.facebook.com/MivzakLive");
        }
        private void toolStripStatusLabel10_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://twitter.com/mivzaklive");
        }
        private void toolStripStatusLabel11_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.youtube.com/user/mivzaklive");
        }
        private void toolStripStatusLabel12_Click(object sender, EventArgs e)
        {
            App.OpenWebSite("http://www.2net.co.il/");
        }
    }
}