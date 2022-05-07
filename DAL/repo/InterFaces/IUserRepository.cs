using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface IUserRepository
    {
        public User ReadUserById(int userId);
        public User ReadUserForSet(string userAccountId);
    }
}