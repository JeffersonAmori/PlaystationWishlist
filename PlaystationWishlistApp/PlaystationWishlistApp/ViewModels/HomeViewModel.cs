using MvvmHelpers;
using PlaystationWishlist.Core.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PlaystationWishlistApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableRangeCollection<PlaystationGame> gameList;

        public HomeViewModel()
        {
            GameList = new ObservableRangeCollection<PlaystationGame>();
        }

        public ObservableRangeCollection<PlaystationGame> GameList { get => gameList; set => SetProperty(ref gameList, value); }

        public override async Task Initialize()
        {
            HttpClient client = new HttpClient();
            
            var response = await client.GetAsync("https://playstationwishlistapi.azurewebsites.net/api/games"); 
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            GameList.AddRange(result);
        
            await Task.FromResult(true);
        }
    }
}
