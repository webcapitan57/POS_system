using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class AdminRepository : IAdminRepository
    {
        private PhotoAppDbContext _ctx;

        public AdminRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public Admin ReadAdminById(int id)
        {
            return _ctx.Admins.Find(id);
        }

        public Admin ReadAdminById(string id)
        {
            return _ctx.Admins.First(a => a.UserAccountId == id);
        }

        public IEnumerable<SetUp> ReadSetUpsOfAdmin(string id)
        {
            return _ctx.SetUpAdmins.Where(sa => sa.Admin.UserAccountId == id).Select(sa => sa.SetUp);
        }

        public int AddAdmin(Admin admin)
        {
            _ctx.Admins.Add(admin);
            _ctx.SaveChanges();
            
            return _ctx.Admins.ToList().Last().UserId;
        }

        public void UpdateAdmin(Admin admin)
        {
            _ctx.Admins.Update(admin);
            _ctx.SaveChanges();
        }

        public void CreateSetupAdmin(SetUpAdmin setUpAdmin)
        {
            _ctx.SetUpAdmins.Add(setUpAdmin);
            _ctx.SaveChanges();
        }

        public SetUpAdmin GetSetupAdmin(int userId, int setupId)
        {
            return _ctx.SetUpAdmins.First(sa =>
                sa.Admin.UserId == userId
                && sa.SetUp.SetUpId == setupId);
        }

        public void RemoveSetupAdmin(SetUpAdmin setupAdmin)
        {
            _ctx.SetUpAdmins.Remove(setupAdmin);
            _ctx.SaveChanges();
        }

        public IEnumerable<Admin> GetKnownAdmins(string userId)
        {
            return _ctx.Admins
                .Include(a => a.UserAccount)
                .Include(a => a.SetUps)
                .ThenInclude(sa => sa.SetUp)
                .Where(a => a.UserAccountId != userId &&
                            a.SetUps.Any(sa =>
                                sa.SetUp.Admins.Any(
                                    sa2 => sa2.Admin.UserAccountId == userId)));
        }
    }
}