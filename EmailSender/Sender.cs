using System;
using PlaystationWishlist.DataAccess.Models;
using PlaystationWishlist.DataAccess.Models.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PlaystationWishlist.EmailSender
{
    public class Sender
    {
    //    public static async void Send()
    //    {
    //        var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
    //        var client = new SendGridClient(apiKey);
    //        var from = new EmailAddress("jefferson_adr@hotmail.com", "Playstation Wishlist");
    //        var subject = "Sending with SendGrid is Fun";
    //        var to = new EmailAddress("jefferson_adr@hotmail.com", "Jeff");
    //        var plainTextContent = "and easy to do anywhere, even with C#";
    //        var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
    //        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
    //        var response = await client.SendEmailAsync(msg);
    //    }

        public static async void Send(PlaystationGame discountedGame, AppUser user)
        {

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jefferson_adr@hotmail.com", "Playstation Wishlist");
            var subject = "PSN game on sale! - Playstation Wishlist";
            var to = new EmailAddress(user.Email, user.Name.Split(" ")[0]);
            var htmlContent = $"The game <strong>{discountedGame.Name}</strong> is on sale for <strong>{discountedGame.Currency}{discountedGame.FinalPrice}</strong>. <br>Check it out: {discountedGame.Url}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}
