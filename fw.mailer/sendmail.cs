using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.ComponentModel;
using System.Net.Mime;
using System.IO;

namespace fw.mailer
{
    class sendmail
    {        
        public static string attachment_path;
        public static string mail_message;
        public static string email_to;       
        public static string email_subject;
        public static string template_option;
        public static string smtp_message;
        


        public static SmtpClient smtpClient = new SmtpClient("mail.fire-wise.com.au");




        public static void Init()
        {   
            smtpClient.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        }


        public static Boolean send()
        {

            Attachment attachment = new Attachment(attachment_path, MediaTypeNames.Application.Pdf);
                                   
            MailAddress from = new MailAddress("Gordon@fire-wise.com.au", "Fire-Wise");
            MailAddress to = new MailAddress(email_to, "");
            MailMessage message = new MailMessage(from.ToString(), to.ToString());

            message.Subject = email_subject;
            message.Body = mail_message;
            message.IsBodyHtml = true;
            message.Attachments.Add(attachment);

            var smtpClient = new SmtpClient("mail.fire-wise.com.au")
            {
                Credentials = new NetworkCredential("invoicing@fire-wise.com.au", "F1r3W1s3!Acc0unts"),  // F1r3W1s3!Acc0unts                                
                EnableSsl = true,                
                Port = 587,
            };

            try
            {
                smtpClient.Send(message);
            }
            catch (SmtpException ex)
            {               
                File.AppendAllText(@"c:\Reports\EmailsSmtp", ex.Message.ToString());
                smtp_message = ex.Message.ToString();
                return false;
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"c:\Reports\EmailsEx", ex.Message.ToString());
                return false;
            }

            return true;

        }




        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.{0}", token);
            }

        }



            
    }
}
