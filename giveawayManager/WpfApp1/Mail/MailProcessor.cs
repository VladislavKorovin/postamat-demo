using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WpfApplication1.GiveawayNamespace;

namespace WpfApplication1.Mail
{
    class MailProcessor
    {
        public static void sendMailMessage(Giveaway giveaway) {
            MailAddress from = new MailAddress("HelpDeskCO.Terminal@leroymerlin.ru");
            MailAddress to = new MailAddress(giveaway.contact.ToString()+"@leroymerlin.ru");
            MailMessage m = new MailMessage(from, to);

            m.Subject = "Выдача оборудования";
            m.Body = String.Format("Произведена выдача оборудования по обращению {0}.\nСообщение отправлено автоматически, отвечать на него не нужно.", giveaway.caseNumber);
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("owa.leroymerlin.ru", 587);
            smtp.Credentials = new NetworkCredential("HelpDeskCO.Terminal", "H9bSQi4RmUc8HGNZ5smBCzFY");
            smtp.EnableSsl = false;
            smtp.Send(m);
        }

        public static void sendMailMessageGiveawayReady(Giveaway giveaway)
        {
            MailAddress from = new MailAddress("HelpDeskCO.Terminal@leroymerlin.ru");
            MailAddress to = new MailAddress(giveaway.contact.ToString() + "@leroymerlin.ru");
            MailMessage m = new MailMessage(from, to);

            m.Subject = "Выдача оборудования";
            m.Body = String.Format("Оборудование по заявке {0} подготовлено. Для получения оборудования, введи код {1} в терминале HelpDeskCO.\nСообщение отправлено автоматически, отвечать на него не нужно.", giveaway.caseNumber, giveaway.id);
            m.IsBodyHtml = false;
            SmtpClient smtp = new SmtpClient("owa.leroymerlin.ru", 587);
            smtp.Credentials = new NetworkCredential("HelpDeskCO.Terminal", "H9bSQi4RmUc8HGNZ5smBCzFY");
            smtp.EnableSsl = false;
            smtp.Send(m);
        }
    }
}
