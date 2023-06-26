using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace fw.mailer
{
    public class alog
    {
        string log_file = @"c:\Reports\email.log";
        public void LogHead()
        {
            if (File.Exists(log_file))
            {
                File.Delete(log_file);
            }
            LogWrite("fw.Mailer - Invoices/Overdues");
            LogWrite("-----------------------------");
        }

        public void LogDone()
        {
            LogWrite("fw.Mailer - Done!");            
        }

        public void LogWrite(string logMessage)
        {            
            try
            {
                using (StreamWriter w = File.AppendText(@"c:\Reports\email.log"))
                {
                    Log(logMessage, w);
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Log(string logMessage, TextWriter txtWriter)
        {
            try
            {
                txtWriter.WriteLine("{0} {1} {2}", DateTime.Now.ToLongTimeString(),
                    DateTime.Now.ToLongDateString(), logMessage);
            }
            catch (Exception ex)
            {
            }
        }

    }
}
