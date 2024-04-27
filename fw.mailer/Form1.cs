using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace fw.mailer
{
    public partial class Form1 : Form
    {
        public DataTable dt = new DataTable();
        public static string the_response;
        public static string fileName;
        public int emailcounter = 0;
        public Stopwatch stopwatch = new Stopwatch();
        public static string am;
        public alog log = new alog();
        //public static bool live_flag;
        public string eBillFolder = @"c:\Reports\eBills\";
        public string email_recipient;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            log.LogHead();
            var e_file = (@"c:\Reports\EmailsSmtp");
            if (File.Exists(e_file))
            {
                File.Delete(e_file);
            }
          

            this.toolStripStatusLabel1.Text = "  ";
            String[] folders = { "" };
           
            if (Directory.Exists(eBillFolder))
            {
                folders = Directory.GetDirectories(eBillFolder).OrderByDescending(s => s).ToArray();
            }
            else { Directory.CreateDirectory(eBillFolder); }

            if (folders.Length > 0)
            { 
                DataTable dt = new DataTable();
                dt.Columns.Add("FoldersName");
                foreach (String f in folders)
                {
                    DirectoryInfo d = new DirectoryInfo(f);
                    dt.Rows.Add(d.Name);
                    comboBox1.Items.Add(d.Name);
                }
                comboBox1.SelectedIndex = 0;               
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)  
        {
            dataGridView1.DataSource = null;
            dt.Clear();
            if (! dt.Columns.Contains("filename")) {
                dt.Columns.Add("filename", typeof(String));
                dt.Rows.Clear();
            }
            DirectoryInfo d = new DirectoryInfo( eBillFolder + comboBox1.SelectedItem.ToString());
            sendmail.template_option = "N";

            if (d.ToString().Contains("Overdues"))
            {
                sendmail.template_option = "O";
            }
            FileInfo[] Files = d.GetFiles("*.pdf");
            foreach (FileInfo file in Files)
            {                  
                dt.Rows.Add(file);
            }           
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            Directory.SetCurrentDirectory(d.FullName);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.toolStripStatusLabel1.Text = "Processing...";
           
            email_recipient = "robert@reeves.net";
            
            backgroundWorker1.RunWorkerAsync();            
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            emailcounter = 0;
            stopwatch.Start();
            foreach (DataRow r in dt.Rows)
            {           
                var splitpos = r[0].ToString().IndexOf("-Invoice");
                if (splitpos != -1)
                {
                        sendmail.email_to = r[0].ToString().Substring(0, splitpos);
                }

                log.LogWrite(sendmail.email_to);
                
                sendmail.attachment_path = r[0].ToString();

                template_me tp = new template_me();
                sendmail.mail_message = tp.get_mailmessage(r[0].ToString());
                sendmail.email_subject = tp.get_subject(@"Message from Fire-Wise");

                if (sendmail.send())
                {
                    emailcounter++;
                }
                else
                {
                    log.LogWrite(sendmail.smtp_message);
                }
                
                backgroundWorker1.ReportProgress(1);
            }
            stopwatch.Stop();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {            
            this.toolStripStatusLabel1.Text = string.Format("Finished - {0} emails took {1} secs", emailcounter, stopwatch.ElapsedMilliseconds / 1000); ;
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.toolStripStatusLabel1.Text = string.Format("{0} of {1} emails sent.",emailcounter,dt.Rows.Count);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            log.LogDone();
            Form f = new Form2();
            f.ShowDialog();
        }
    }
}
