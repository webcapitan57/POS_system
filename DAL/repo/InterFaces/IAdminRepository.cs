using System.Collections.Generic;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace POC.DAL.repo.InterFaces
{
    public interface IAdminRepository
    {
        public Admin ReadAdminById(int id);
        public Admin ReadAdminById(string id);
        public IEnumerable<SetUp> ReadSetUpsOfAdmin(string id);
        public int AddAdmin(Admin admin);
        public void UpdateAdmin(Admin admin);
        public void CreateSetupAdmin(SetUpAdmin setUpAdmin);
        public SetUpAdmin GetSetupAdmin(int userId, int setupId);
        public void RemoveSetupAdmin(SetUpAdmin setupAdmin);
        public IEnumerable<Admin> GetKnownAdmins(string userId);
    }
}