using AutoMapper;
using PlaystationWishlist.Core.Interfaces;
using PlaystationWishlist.DataAccess.Data;
using System.Linq;
using System.Threading.Tasks;

namespace PlaystationWishlist.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PlaystationWishlistContext _context;
        private IUserManager _userManager;
        private readonly IMapper _mapper;

        public UserRepository(PlaystationWishlistContext context, IUserManager userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> GetOrCreateExternalUserAsync(Core.Entities.UserProfile userProfiler)
        {
            if (userProfiler != null)
            {
                Models.UserProfile user = _context.UserProfiles.FirstOrDefault(x => x.OId == userProfiler.OId && x.OIdProvider == userProfiler.OIdProvider);

                if (user == null)
                {
                    user = _mapper.Map<Models.UserProfile>(userProfiler);
                    await _context.UserProfiles.AddAsync(user);
                    await _context.SaveChangesAsync();
                }

                // optionally call userManager.SignIn() 
                // to setup additional claims apart from the ones
                // received from the social login
                // await userManager.SignIn(httpContext, user);

                return true;
            }

            return false;
        }
    }
}