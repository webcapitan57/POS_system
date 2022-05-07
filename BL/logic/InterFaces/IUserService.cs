using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface IUserService
    {
        public User GetUser(int userId);
        public FilterProfile GetFilterProfile(int filterProfileId);
        public void AddFilterProfile(FilterProfile filterProfile);
        public User GetUserForSet(string userAccountId);
    }
}