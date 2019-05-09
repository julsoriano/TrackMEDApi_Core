using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace TrackMEDApi.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link http://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            // Ref http://stackoverflow.com/questions/33496290/how-to-send-email-by-using-mailkit
            var emailMessage = new MimeMessage();

	        emailMessage.From.Add(new MailboxAddress("Julito Soriano", "jul_soriano@hotmail.com"));
	        emailMessage.To.Add(new MailboxAddress("Pita", email));
	        emailMessage.Subject = subject;
	        emailMessage.Body = new TextPart("plain") { Text = message };
      
	        using (var client = new SmtpClient())
	        {
                client.Connect("smtp.live.com", 587, false);  // Error: The remote certificate is invalid according to the validation procedure

                // http://www.mimekit.net/docs/html/T_MailKit_Net_Smtp_SmtpClient.htm
                //client.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect); // Error: Existing connection forcibly closed by the remote host
                //client.Connect("smtp.gmail.com", 587, SecureSocketOptions.None); // Connection attempt because connected party did not respond in time

                // Note: only needed if the SMTP server requires authentication. See http://stackoverflow.com/questions/33496290/how-to-send-email-by-using-mailkit
                client.Authenticate("jul_soriano@hotmail.com", "acts15:23hot");

                client.Send(emailMessage);
                client.Disconnect(true);

                /* async version
		        client.LocalDomain = "smtp.live.com";                
		        client.ConnectAsync("smtp.relay.uri", 587, SecureSocketOptions.None).ConfigureAwait(false);  // 25
		        client.SendAsync(emailMessage).ConfigureAwait(false);
		        client.DisconnectAsync(true).ConfigureAwait(false);                
                */
            }

            /*
            using (StreamWriter data = System.IO.File.CreateText("c:\\smtppickup\\email.txt"))
            {
                emailMessage.WriteTo(data.BaseStream);
            }*/

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
