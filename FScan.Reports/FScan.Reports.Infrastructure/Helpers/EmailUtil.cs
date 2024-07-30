using FScan.Reports.Application.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FScan.Reports.Infrastructure.Helpers;
public class EmailUtil
{
    public string SendMail(MailDTO maildetails)
    {
        string message = string.Empty;
        try
        {

            using (SmtpClient smtpClient = SMTPClient())
            {

                using (MailMessage mail = new MailMessage())
                {
                    //subject
                    mail.Subject = maildetails.Subject;
                    //from
                    mail.From = new MailAddress(maildetails.MailFrom, maildetails.MailFrom);
                    //to
                    mail.To.Add(new MailAddress(maildetails.MailTo, maildetails.MailTo));

                    mail.IsBodyHtml = true;
                    mail.Body = maildetails.Body;


                    //email send
                    smtpClient.Send(mail);


                }
            }
        }

        catch (Exception ex)
        {
            //ViewData["Error"] = "Error: " + ex.Message;
            return message;
        }

        return message;

    }

    private static SmtpClient SMTPClient()
    {
        var SMTPPORT = "25";

        if (SMTPPORT == "25")
        {
            return new SmtpClient(Settings.Config.SMTP_HOST);
        }
        else
        {
            return new SmtpClient(Settings.Config.SMTP_HOST, Convert.ToInt32(Settings.Config.SMTP_PORT));
        }
    }
}