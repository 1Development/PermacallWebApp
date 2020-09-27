using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using PCDataDLL;

namespace TSLogger
{
    class MailClass
    {
        public static void SendNotificationMail(string mailBody)
        {
            var fromAddress = new MailAddress("host.youri@gmail.com", "HOST");
            var toAddress = new MailAddress("yourish@live.nl", "Youri Schuurmans");
            string fromPassword = SecureData.HostEmailPassword;
            string subject = "SERVER ERROR";
            string bodyPrefix = "The server has encountered an error: \r\n\r\n";
            string body = bodyPrefix+ mailBody;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
