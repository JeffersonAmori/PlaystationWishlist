using System;
using System.IO;
using System.Reflection;
using PlaystationWishlist.DataAccess.Models;
using PlaystationWishlist.DataAccess.Models.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace PlaystationWishlist.EmailSender
{
    public class Sender
    {
        public static async void Send(PlaystationGame discountedGame, AppUser user)
        {
            string emailText = ProcessEmail(discountedGame);

            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("noreply@playstationwishlist.com", "Playstation Wishlist");
            var subject = "PSN game on sale! - Playstation Wishlist";
            var to = new EmailAddress(user.Email, user.Name.Split(" ")[0]);
            //var htmlContent = $"The game <strong>{discountedGame.Name}</strong> is on sale for <strong>{discountedGame.Currency}{discountedGame.FinalPrice}</strong>. <br />Check it out: {discountedGame.Url}";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, emailText);
            await client.SendEmailAsync(msg);
        }

        private static string ProcessEmail(PlaystationGame discountedGame)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "PlaystationWishlist.EmailSender.email.html";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string emailText = reader.ReadToEnd()
                    .Replace("{{discountedGameName}}", discountedGame.Name)
                    .Replace("{{discountedGameCurrency}}", discountedGame.Currency)
                    .Replace("{{discountedGameFinalPrice}}", discountedGame.FinalPrice.ToString())
                    .Replace("{{discountedGameImageUrl}}", discountedGame.GameImageUrl)
                    .Replace("{{discountedGameUrl}}", discountedGame.Url);

                return emailText;
            }
        }
    }
}
