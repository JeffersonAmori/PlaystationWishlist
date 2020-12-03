using System;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PlaystationWishlist.EmailSender
{
    public class Sender
    {
        public static async void Send()
        {
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jefferson_adr@hotmail.com", "Playstation Wishlist");
            var subject = "Sending with SendGrid is Fun";
            var to = new EmailAddress("jefferson_adr@hotmail.com", "Jeff");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
