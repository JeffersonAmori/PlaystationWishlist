using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlaystationWishlist.Core.Entities;

namespace PlaystationWishlist.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> GetOrCreateExternalUserAsync(UserProfile userProfiler);
    }
}
