using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        int timer_init = 70000;
        int auto_search = 0;
        int counter = 0;
        string search_class;
        string[] search_subject=new string[100];
        string  ctext,ltext;
        int cnum, lnum;
        public Form1()
        {
            InitializeComponent();
        }


        private void Form1_Resize(object sender, EventArgs e)
        {

            webBrowser1.Size = new Size((this.Size.Width) - 50,(this.Size.Height) - 50);

        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            int kieru = Convert.ToInt32((DateTime.Now.ToString("yyyyMMdd")));
            if (kieru > 20201231) Application.Exit();
            /*
            webBrowser1.Navigate("http://www.ntust.edu.tw/files/13-1000-21427.php");            
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete))  //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            while (((webBrowser1.Url.ToString()) != "http://www.ntust.edu.tw/files/13-1000-21427.php")) ;
             * */

            webBrowser1.Navigate("http://140.118.31.215/querycourse/querycondition.aspx");
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete))  //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            while (((webBrowser1.Url.ToString()) != "http://140.118.31.215/querycourse/querycondition.aspx")) ;
            webBrowser1.Navigate("https://qcourse.ntust.edu.tw/querycourse/ChCourseQuery/QueryCondition.aspx");

            timer1.Interval = timer_init;
            label1.Text = timer_init.ToString();

            sign_in();
            textBox2.Text = "";
        }
        private void sign_in()
        {
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete))  //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            HtmlDocument doc = webBrowser1.Document;
            //doc.GetElementById("Checkbox_G").SetAttribute("checked", "checked");//"checked" " "取消check
            doc.GetElementById("Ctb0101").SetAttribute("value", "ET5229701");
            HtmlElement MyButton = doc.All["QuerySend"];
            MyButton.InvokeMember("click");
            textBox2.Text = "";
        }
        private void search()
        {
            timer1.Enabled = false;
            int j;
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete))  //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            HtmlDocument doc = webBrowser1.Document;
            if ((webBrowser1.Url.ToString()) != "https://qcourse.ntust.edu.tw/querycourse/ChCourseQuery/QueryCondition.aspx")
            {
                for (int i = 0; i < doc.All.Count; i++)
                {
                    if (doc.All[i].GetAttribute("href").Contains(search_class))
                    {
                        
                        char[] cwords;
                       // textBox2.AppendText(doc.All[10 + i].InnerText);
                       // textBox2.AppendText(doc.All[13 + i].InnerText);
                        ctext = doc.All[10 + i].InnerText;
                        cwords = ctext.ToCharArray(0, ctext.Length);
                        while (cwords[0] <'0' || cwords[0]>'9' )
                        {
                            i++;
                            ctext = doc.All[10 + i].InnerText;
                            cwords = ctext.ToCharArray(0, ctext.Length);
                        }
                        if(ctext.Length==1)
                        {  
                            cwords= ctext.ToCharArray(0, 1);
                            cnum=(Convert.ToInt32(cwords[0])-48);
                        }
                        else if(ctext.Length==2)
                        {  
                            cwords= ctext.ToCharArray(0, 2);
                            cnum = (Convert.ToInt32(cwords[0]) - 48) * 10 + (Convert.ToInt32(cwords[1]) - 48);
                        }
                        else if (ctext.Length == 3)
                        {
                            cwords = ctext.ToCharArray(0, 3);
                            cnum = (Convert.ToInt32(cwords[0]) - 48) * 100 + (Convert.ToInt32(cwords[1]) - 48) * 10 + (Convert.ToInt32(cwords[2]) - 48);
                        }
                        ltext = doc.All[13 + i].InnerText;
                        char[] lwords = ltext.ToCharArray(0, ltext.Length);
                        for (j=0; j < ltext.Length;j++ )
                        {
                            if (lwords[j] == 38480) break;
                        }
                        lwords = ltext.ToCharArray(j+1, 3);
                        if (lwords[1] > '9' || lwords[1] < '0')
                        {
                            lwords = ltext.ToCharArray(j + 1, 1);
                            lnum = (Convert.ToInt32(lwords[0]) - 48);
                        }
                        else if (lwords[2] > '9' || lwords[2] < '0')
                        {
                            lwords = ltext.ToCharArray(j + 1, 2);
                            lnum = (Convert.ToInt32(lwords[0]) - 48) * 10 + (Convert.ToInt32(lwords[1]) - 48);
                        }
                        else
                        {
                            lwords = ltext.ToCharArray(j + 1, 3);
                            lnum = (Convert.ToInt32(lwords[0]) - 48) * 100 + (Convert.ToInt32(lwords[1]) - 48) * 10 + (Convert.ToInt32(lwords[2]) - 48);

                        }
                        textBox2.AppendText(" ");
                        textBox2.AppendText((cnum.ToString("###")));
                        textBox2.AppendText(" ");
                        textBox2.AppendText((lnum.ToString("###")));
                        textBox2.AppendText(" ");
                        if (cnum < lnum)
                        {
                            textBox3.Text = search_class + "\n";
                            textBox2.AppendText("可加選");
                            
                            MessageBox.Show(search_class + "可加選");

                        }
                        else textBox2.AppendText("已滿額");
                        textBox2.AppendText("\n");
                        //  textBox2.AppendText(doc.All[i].GetAttribute("href"));

                        break;
                    }
                }
            }
            if (auto_search==1) timer1.Enabled = true;
        }

        public class SoundClass
        {
            [DllImport("winmm.dll")]
            private static extern int PlaySound(string name, int hmod, int flags);
            public const int SND_SYNC = 0x0;
            public const int SND_ASYNC = 0x1;
            public const int SND_FILENAME = 0x20000;
            public const int SND_RESOURCE = 0x40004;
            public void PlaySoundFile(string filename)
            {
                PlaySound(filename, 0, SND_FILENAME | SND_ASYNC);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            textBox2.Text=("");
            for (int i = 0; i < counter;i++ )
            {
                textBox2.Text = textBox2.Text+ search_subject[i];
                search_class = search_subject[i];
                search();
                textBox2.Text = textBox2.Text + "\n";
            }
                
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int time_msec = ((rnd.Next()) % 10000);

            timer1.Interval = timer_init + time_msec;
            label1.Text = (timer_init + time_msec).ToString();

            for (int i = 0; i < 5000; i++) ;
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete)) //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            if ((webBrowser1.Url.ToString()) == "https://qcourse.ntust.edu.tw/querycourse/ChCourseQuery/QueryCondition.aspx")
            {
                sign_in();
                timer1.Interval = 5000;
                textBox2.Text = ("");
            }
            while (!(webBrowser1.ReadyState == WebBrowserReadyState.Complete)) //判斷網頁是否載入完成
            {
                Application.DoEvents();
            }
            if ((webBrowser1.Url.ToString()) != "https://qcourse.ntust.edu.tw/querycourse/ChCourseQuery/QueryCondition.aspx")
            {
                if (textBox1.Text != "" && textBox2.Text == "")
                {
                    timer1.Enabled = false;
                    textBox2.Text = ("");
                    for (int i = 0; i < counter; i++)
                    {
                        textBox2.Text = textBox2.Text + search_subject[i] + " ";
                        search_class = search_subject[i];
                        search();
                        textBox2.Text = textBox2.Text + "\n";
                    }
                }
                else
                {
                    webBrowser1.Refresh();
                }

            }
            /*
        else if ((webBrowser1.Url.ToString()) == "https://qcourse.ntust.edu.tw/querycourse/ChCourseQuery/QueryCondition.aspx")
        {
            sign_in();
            timer1.Interval = 5000;
            textBox2.Text = ("");
        }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(auto_search==0)
            {
                auto_search = 1;
                add.Enabled = false;
                timer1.Enabled = true;
                button1.Enabled = false;
                textBox2.Text = ("");
                for (int i = 0; i < counter; i++)
                {
                    textBox2.Text = textBox2.Text + search_subject[i];
                    search_class = search_subject[i];
                    search();
                    textBox2.Text = textBox2.Text + "\n";
                }
            }
            else
            {
                auto_search = 0;
                add.Enabled = true;
                timer1.Enabled = false;
                button1.Enabled = true;
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            counter = 0;
            textBox1.Text="";
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"..\..\subject.txt");
            while ((search_subject[counter] = file.ReadLine()) != null)
            {
                counter++;
            }

            file.Close();
            for (int i = 0; i < counter; i++)
                textBox1.Text = textBox1.Text + search_subject[i] + "\n";
        }
    }
}
