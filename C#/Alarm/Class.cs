using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
namespace Alarm
{
    public class Variables
    {
        public static System.Collections.Hashtable text = new System.Collections.Hashtable();
        public static System.Collections.Hashtable setting = new System.Collections.Hashtable();
        public static string[,] readKeys =
        {
            {"newline", "\r\n"}
        };
    }
    public class App
    {
        public static string
            path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + "/mivzaklive",
            version = Alarm.Properties.Resources.Version,
            website = "http://mivzaklive.co.il",
            programurl = "http://mivzaklivenews.t15.org/",
            ads = "http://google.com",
            mail = "mivzaklive@gmail.com",
            rssurl = "http://www.facebook.com/feeds/page.php?id=201182249942365&format=rss20";
        public static string[] files = { "setting.cfg", "he.lang", "en.lang", "ru.lang", "ar.lang", "default.wav", "alarm.wav" };
        public static void OpenWebSite(string url)
        {
            System.Diagnostics.Process.Start(url);
        }
        public static string ReadFromWeb(string site)
        {
            WebClient x = new WebClient();
            System.IO.Stream y = x.OpenRead(site);
            System.IO.StreamReader z = new System.IO.StreamReader(y, Encoding.Default);
            string s = z.ReadToEnd();
            z.Close();
            y.Close();
            z.Dispose();
            y.Dispose();
            x.Dispose();
            return s;
        }
        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        public static void ReadConfiguration(string filepath, char seperator, ref System.Collections.Hashtable output)
        {
            output.Clear();
            string[] lines = File.ReadAllLines(filepath, Encoding.Default);
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].Length > 1)
                    output[lines[i].Split(seperator)[0]] = ReadK(lines[i].Remove(0, lines[i].IndexOf(seperator) + 1));
        }
        public static void UpdateConfiguration(string filepath, char seperator, ref System.Collections.Hashtable output, string from, string to)
        {
            string txt = File.ReadAllText(filepath);
            File.WriteAllText(filepath, txt.Replace(from + seperator + output[from].ToString(), from + seperator + to));
            output[from] = to;
        }
        public static string ReadK(string line)
        {
            for (int i = 0; i < Variables.readKeys.GetLength(0); i++)
                if (line.Contains("{" + Variables.readKeys[i, 0] + "}"))
                    line = line.Replace("{" + Variables.readKeys[i, 0] + "}", Variables.readKeys[i, 1]);
            return line;
        }
        public static string INIGetKey(string filepath, string key, bool pathsync = true)
        {
            string[] lines = File.ReadAllLines(filepath);
            for (int i = 0; i < lines.Length; i++)
                if (lines[i].StartsWith(key + "="))
                    return pathsync ? lines[i].Remove(0, key.Length + 1).Replace("PATH",programurl) : lines[i].Remove(0, key.Length + 1);
            return string.Empty;
        }
        public static bool CheckFiles(int testid)
        {
            switch (testid)
            {
                case 1:
                    {
                        try
                        {
                            string[] lines = File.ReadAllLines(App.path + "/setting.cfg");
                            if (lines.Length != 13) return false;
                            int[] lens = new int[] { 3214, 3284, 8040, 6588 };
                            FileInfo fi;
                            for (int i = 0; i < lens.GetLength(0); i++)
                            {
                                fi = new FileInfo(path + "/" + files[i + 1]);
                                if ((int)fi.Length != lens[i]) return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            string[,] settingLimits = new string[,]
                            {
                                {"sound","0|1","1"},
                                {"startup","0|1","0"},
                                {"not1","0|1","1"},
                                {"not2","0|1","1"},
                                {"not3","0|1","1"},
                                {"not4","0|1","1"},
                                {"language","he|en|ar|ru","he"},
                                {"side","right|left","right"}
                            };
                            string[] splt;
                            bool flag = false;
                            for (int i = 0; i < settingLimits.GetLength(0); i++)
                            {
                                splt = settingLimits[i, 1].Split('|');
                                flag = false;
                                for (int j = 0; j < splt.GetLength(0) && !flag; j++)
                                    if (Variables.setting[settingLimits[i, 0]].ToString() == splt[j])
                                        flag = true;
                                if (!flag) App.UpdateConfiguration(App.path + "/setting.cfg", ' ', ref Variables.setting, settingLimits[i, 0], settingLimits[i, 2]);
                            }
                        }
                        catch
                        {
                            return false;
                        }
                        break;
                    }
            }
            return true;
        }
        public static int NumOfNotType(Notification.NotificationTypes type)
        {
            switch (type)
            {
                case Notification.NotificationTypes.Alarm: return 1;
                case Notification.NotificationTypes.Emergency: return 2;
                case Notification.NotificationTypes.Security: return 3;
                case Notification.NotificationTypes.Any: return 4;
                default: return 0;
            }
        }
        public static bool IsValidEmail(string mail)
        {
            return Regex.IsMatch(mail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        public static bool Mail(string to, string subject, string body)
        {
            return ReadFromWeb(programurl + "mail.php?to=" + to + "&subject=" + subject + "&body=" + body).Contains("TRUE");
        }
    }
    public class Notification
    {
        public enum NotificationTypes { None, Alarm, Emergency, Security, Any }
        public string title = string.Empty;
        public string time = string.Empty;
        public NotificationTypes type = NotificationTypes.None;
        public string description = string.Empty;
        public string link = string.Empty;
        public string sitelink = string.Empty;
        public Notification(string title, string time, NotificationTypes type, string description, string link, string sitelink)
        {
            this.title = title;
            this.time = time;
            this.type = type;
            this.description = description;
            this.link = link;
            this.sitelink = sitelink;
        }
    }
    public class RSS
    {
        private XmlNode rssNode = null, channelNode = null;
        public List<XmlNode> items = new List<XmlNode>();
        private string url = string.Empty;
        private bool onlyAlarms = false;
        //public XmlNode last = null;
        public RSS(string url, bool onlyAlarms)
        {
            this.url = url;
            this.onlyAlarms = onlyAlarms;
            ReadData();
        }
        public void ReadData()
        {
            if (!App.IsConnectedToInternet())
            {
                new Error("ישנה בעיה בחיבור לאינטרנט, לא ניתן להמשיך את השימוש בתוכנה.").ShowDialog();
                return;
            }
            string way = "";
            try
            {
                way = "A";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                way = "B";
                request.UserAgent = "Alarm";
                way = "C";
                XmlDocument doc = new XmlDocument();
                way = "D";
                using (WebResponse response = request.GetResponse())
                using (Stream rss = response.GetResponseStream())
                {
                    doc.Load(rss);
                    rss.Close();
                    rss.Dispose();
                    response.Close();
                }
                way = "E";
                for (int i = 0; i < doc.ChildNodes.Count && rssNode == null; i++)
                    if (doc.ChildNodes[i].Name == "rss")
                    {
                        rssNode = doc.ChildNodes[i];
                        way = "F" + i;
                    }
                for (int i = 0; i < rssNode.ChildNodes.Count && channelNode == null; i++)
                    if (rssNode.ChildNodes[i].Name == "channel")
                    {
                        channelNode = rssNode.ChildNodes[i];
                        way = "G" + i;
                    }
                for (int i = 0; i < channelNode.ChildNodes.Count; i++)
                    if (channelNode.ChildNodes[i].Name == "item")
                    {
                        items.Add(channelNode.ChildNodes[i]);
                        way = "H" + i;
                        //if (last == null) last = channelNode.ChildNodes[i];
                    }
                doc.RemoveAll();
                request.Abort();
            }
            catch
            {
                new Error("קיימת בעיה בעדכון תוכן, אם הבעיה מתמשכת פנה אלינו במייל: " + App.mail + "\r\n(קוד בעיה: " + way + ")").ShowDialog();
            }
        }
        public void Refresh()
        {
            rssNode = null;
            channelNode = null;
            //last = null;
            items.Clear();
            ReadData();
        }
        public List<XmlNode> GetLastItems(Notification.NotificationTypes gettype, int count)
        {
            List<XmlNode> returnedList = new List<XmlNode>();
            for (int i = 0; i < count; i++)
                if (gettype == Notification.NotificationTypes.Any || ConvertToNotification(items[i]).type == gettype)
                    returnedList.Add(items[i]);
            return returnedList;
        }
        public int Count()
        {
            return items.Count;
        }
        public static Notification ConvertToNotification(XmlNode item)
        {
            string title = string.Empty, time = item["pubDate"].InnerText, desc = item["description"].InnerText, slink = string.Empty;
            string[] months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            time = time.Replace(" +0000", "");
            time = time.Remove(0, time.IndexOf(' ') + 1);
            for (int i = 0; i < months.Length; i++)
                if (time.Contains(months[i]))
                    time = time.Replace(" " + months[i] + " ", "/" + (i <= 8 ? "0" : "") + (i + 1) + "/");
            DateTime dt = Convert.ToDateTime(time.Split(' ')[1]);
            dt = dt.AddHours(2.0);
            time = string.Format("{0} {1}:{2}:{3}",
                time.Split(' ')[0],
                dt.Hour <= 9 ? ("0" + dt.Hour) : dt.Hour.ToString(),
                dt.Minute <= 9 ? ("0" + dt.Minute) : dt.Minute.ToString(),
                dt.Second <= 9 ? ("0" + dt.Second) : dt.Second.ToString());
            if (desc.Contains("http://www.mivzaklive.co.il/archives/"))
            {
                //       <description><![CDATA[<a href="http://www.mivzaklive.co.il/archives/27820#.T9C1i_hra1E.facebook" id="" title="" target="" onclick="" style="" rel="nofollow" onmousedown="UntrustedLink.bootstrap($(this), &quot;cAQG_6Fhx&quot;, event, bagof(&#123;&#125;));"><img class="img" src="https://fbexternal-a.akamaihd.net/safe_image.php?d=AQDswiAIXV4zjhhw&amp;w=90&amp;h=90&amp;url=http%3A%2F%2Fwww.mivzaklive.co.il%2Fwp-content%2Fuploads%2F2012%2F06%2FMivzakLive78.jpg" alt="" /></a><br/><a href="http://www.mivzaklive.co.il/archives/27820#.T9C1i_hra1E.facebook" id="" target="_blank" style="" rel="nofollow" onmousedown="UntrustedLink.bootstrap($(this), &quot;QAQEB2-V1&quot;, event, bagof(&#123;&#125;));">ראש העין : 2 חשודים שדדו אלפי שקלים מסניף דואר ונמלטו – אין נפגעים</a><br/>‪www.mivzaklive.co.il‬<br/>לפני זמן קצר (חמישי) אירע שוד בסניף דואר, באזור התעשייה &#039;אפק&#039; בראש העין. 2 חשודים ...]]></description>
                slink = desc.Remove(0, desc.IndexOf('\"') + 1);
                slink = slink.Remove(slink.IndexOf('\"'));
            }
            desc = desc.Replace("<br />", "\r\n");
            while (desc.Contains("<"))
            {
                int i = desc.IndexOf('<'), j = desc.IndexOf('>');
                desc = desc.Remove(i, (j - i) + 1);
            }
            title = desc;
            if (title.Length >= 50)
            {
                title = title.Remove(48);
                title += "...";
            }
            title = title.Replace("&quot;", "\"");
            desc = desc.Replace("&quot;", "\"");
            return new Notification(title, time, DetectNotType(title), desc, item["link"].InnerText, slink);
        }
        public static Notification.NotificationTypes DetectNotType(string text)
        {
            string[,] keywords =
            {
                {"אזעקת צבע אדום","A"},
                {"אזעקה צבע אדום","A"},
                {"נא להתמגן","A"},
                {"יציאות לכיוון","A"},
                {"יציאות לעבר","A"},
                {"נמסר על","B"},
                {"תאונת עבודה","B"},
                {"אירוע ירי","B"},
                {"אירוע דקירה","B"},
                {"פיגוע","B"},
                {"אירוע חומ\"ס","B"},
                {"אירוע פח\"ע","B"},
                {"זריקת אבנים","B"},
                {"נפילה מגובה","B"},
                {"אירוע שוד","B"},
                {"התאבדות","B"},
                {"עולה באש","B"},
                {"אר\"ן","B"},
                {"אירוע רב נפגעים","B"},
                {"פיגוע","C"},
                {"צה\"ל תוקף","C"},
                {"חיל האויר תוקף","C"},
                {"הועלתה הכוננות","C"},
                {"התקבלה התראה לירי","C"},
                {"אביר לילה","C"},
                {"קוד פיגיון","C"},
                {"קוד פגיון","C"},
                {"פרש תורכי","C"},
                {"פרש טורקי","C"}
            };
            for (int i = 0; i < keywords.GetLength(0); i++)
                if (text.Contains(keywords[i, 0]))
                    switch (keywords[i, 1])
                    {
                        case "A": return Notification.NotificationTypes.Alarm;
                        case "B": return Notification.NotificationTypes.Emergency;
                        case "C": return Notification.NotificationTypes.Security;
                        default: return Notification.NotificationTypes.Any;
                    }
            return Notification.NotificationTypes.Any;
        }
    }
}