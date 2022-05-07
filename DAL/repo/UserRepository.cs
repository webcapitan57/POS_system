using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class UserRepository :IUserRepository
    {

        private readonly PhotoAppDbContext _ctx = null;

        public UserRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx ;
        }
        public User ReadUserById(int userId)
        {
            switch (_ctx.AppUsers.Find(userId))
            {
                case Teacher teacher:
                    return _ctx.Teachers.Include(t => t.Groups).ToList().Find(t=>t.UserId==userId);
                
                case Admin admin:
                    return null;
                
            }

            return null;
        }

        public User ReadUserForSet(string userAccountId)
        {
            return _ctx.AppUsers
                .Include(t => t.QualityScoreEvents)
                .ThenInclude(e => e.Answer)
                .Include(t => t.FilterProfiles)
                .FirstOrDefault(t => t.UserAccountId == userAccountId);
        }
    }
}