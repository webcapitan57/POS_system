using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFilterProfileRepository _filterProfileRepository;

        public UserService(IUserRepository userRepository, IFilterProfileRepository filterProfileRepository)
        {
            _userRepository = userRepository;
            _filterProfileRepository = filterProfileRepository;
        }

        public User GetUser(int userId)
        {
            return _userRepository.ReadUserById(userId);
        }

        public FilterProfile GetFilterProfile(int filterProfileId)
        {
            return _filterProfileRepository.ReadFilterProfile(filterProfileId);
        }

        public void AddFilterProfile(FilterProfile filterProfile)
        {
            _filterProfileRepository.AddFilterProfile(filterProfile);
        }

        public User GetUserForSet(string userAccountId)
        {
            return _userRepository.ReadUserForSet(userAccountId);
        }
    }
}