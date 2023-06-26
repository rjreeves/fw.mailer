using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fw.mailer
{
    public class template_me
    {  

        public string email_template = @"
            <html>
            <h3>Fire Wise</h3>           

            Hi <b>{{custname}}</b>
          
            <br>Your invoice/reference number is <b>{{invoicenumber}}</b> . . . please use this if paying by direct deposit.
            <br>
            <br>Your Fire Wise subscription renewal invoice is attached.
            <br>Please ensure this payment is made promptly.
            <br>
            <br>Any changes should be notified by reply email.
            <br>
            <br>Gordon King
            <br>gordon@fire-wise.com.au
                       
            </html>";


        public string email_template_overdue = @"
            <html>
            <h3>Fire Wise</h3>           

            Hi <b>{{custname}}</b>
          
            <br>Your reference number is <b>{{invoicenumber}}</b> . . . please use this if paying by direct deposit.
            <br>
            <br>Your Fire Wise overdue invoice is attached.
            <br>Please ensure this payment is made promptly.
            <br>
            <br>Any changes should be notified by reply email.
            <br>
            <br>Gordon King
            <br>gordon@fire-wise.com.au
                       
            </html>";

        public string get_mailmessage(string fileName)
        {
            var new_filename = fileName.Replace("-Invoice", "|");
            var email_address = new_filename.Split('|')[0].ToString();
            var t_cust_no = new_filename.Split('|')[1].ToString();
            var cust_no = t_cust_no.Split('-')[0].ToString();
            var invoice_number = t_cust_no.Split('-')[1].ToString();
            invoice_number = invoice_number.Replace(".pdf", ""); //remolve the filename extension

            var htmlContent = email_template;

            if (sendmail.template_option == "O")
            {
                htmlContent = email_template_overdue;
            }

            htmlContent = htmlContent.Replace("{{invoicenumber}}", cust_no);   // Business rule by Gordon to print Cust No for invoice.
            htmlContent = htmlContent.Replace("{{custname}}", email_address);

            return htmlContent;
        }

        public string get_subject(string default_subject)
        {
            var subject = "Fire Wise Subscription Invoice";
            if (sendmail.template_option == "O")
            {
                subject = "Fire Wise Overdue Notice";
            }

            return subject;
        }

    }
}
