using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlaystationWishlist.Core.Entities;

namespace PlaystationWishlist.Core.Interfaces
{
    public interface IUserManager
    {
        Task SignIn(UserProfile user, bool isPersistent = false);
        Task SignOut();
    }
}
