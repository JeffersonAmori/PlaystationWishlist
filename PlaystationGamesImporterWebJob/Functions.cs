using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PlaystationGamesLoadScrapper;

namespace WebJobsSDKSample
{
    public class Functions
    {
        public static void ProcessQueueMessage([QueueTrigger("queue")] string message, ILogger logger)
        {
            logger.LogInformation(message);
            PlaystationStoreScrapper.GetAllGames();
        }
    }
}