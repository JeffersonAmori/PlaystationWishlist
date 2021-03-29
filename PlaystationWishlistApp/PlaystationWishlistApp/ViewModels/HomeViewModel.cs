using MvvmHelpers;
using PlaystationWishlist.Core.Entities;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PlaystationWishlistApp.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private ObservableRangeCollection<PlaystationGame> _gameList;
        private ICommand _addOrRemoveToWishlistCommand;

        public HomeViewModel()
        {
            GameList = new ObservableRangeCollection<PlaystationGame>();
        }

        public ObservableRangeCollection<PlaystationGame> GameList { get => _gameList; set => SetProperty(ref _gameList, value); }
        public ICommand AddOrRemoveToWishlistCommand { get => _addOrRemoveToWishlistCommand; set => SetProperty(ref _addOrRemoveToWishlistCommand, value); }

        public override async Task Initialize()
        {
            IsBusy = true;

            HttpClient client = new HttpClient();

            var response = await client.GetAsync("https://playstationwishlistapi.azurewebsites.net/api/games");
            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PlaystationWishlist.Core.Entities.PlaystationGame>>(
                await response.Content.ReadAsStringAsync());

            GameList.AddRange(result);

            IsBusy = false;

            await Task.FromResult(true);
        }
    }
}
