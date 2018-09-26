using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalWorkshop.Context;
using MailKit.Net.Smtp;
using MimeKit;

namespace FinalWorkshop.Services
{
	public class MailService
	{

		public void SendEmail(string receiver, string subject, string msgText)
		{
			var message = new MimeMessage();

			message.From.Add(new MailboxAddress("PL", "COMPANY-MAIL@gmail.com"));
			message.To.Add(new MailboxAddress(receiver, receiver));
			message.Subject = subject;

			message.Body = new TextPart("plain")
			{
				Text = msgText
			};

			using (var client = new SmtpClient())
			{
				client.Connect("smtp.gmail.com", 587, false);
				client.Authenticate("COMPANY-MAIL@gmail.com", "PASSWORD");
				client.Send(message);
				client.Disconnect(true);
			}
		}

	}
}
