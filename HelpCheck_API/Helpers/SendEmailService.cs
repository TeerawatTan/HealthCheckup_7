using System.Threading.Tasks;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.OtpAndSendMails;
using HelpCheck_API.Constants;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Net;
using MailKit.Security;

namespace HelpCheck_API.Helpers
{
    public class SendEmailService
    {
        public static ResultResponse SendMail(MailRequest request)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(request.Sender, request.SenderEmail));
                message.To.Add(new MailboxAddress("", request.ReceiverEmail));
                message.Subject = request.Subject;

                var builder = new BodyBuilder { HtmlBody = request.Details };
                if (request.Attachments != null)
                {
                    WebClient client = new WebClient();
                    foreach (FileDetails fd in request.Attachments)
                    {
                        Uri uri = new Uri(fd.FilePath);
                        byte[] dbytes = client.DownloadData(uri);
                        builder.Attachments.Add(fd.FileName, dbytes);
                    }
                }
                message.Body = builder.ToMessageBody();

                IConfigurationSection configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("SMTPService");
                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    int smtpPort = Convert.ToInt32(configuration["Port"]);

                    client.Connect(configuration["SMTP"], smtpPort, false);
                    client.Authenticate(configuration["Email"], configuration["Password"]);
                    client.Send(message);
                    client.Disconnect(true);
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_SUCCESS
                };
            }

            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }
    }
}