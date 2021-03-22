using MvvmHelpers;
using System.Threading.Tasks;

namespace PlaystationWishlistApp.ViewModels
{
    public class ViewModelBase : BaseViewModel
    {
        public virtual async Task Initialize() { }
    }
}
