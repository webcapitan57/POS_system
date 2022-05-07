using System.Collections.Generic;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface IAdminService
    {
        public Admin GetAdmin(int id);
        public Admin GetAdmin(string id);
        public IEnumerable<SetUp> GetSetUpsOfAdmin(string id);
        public void UpdateAdmin(Admin admin);
        public int AddAdmin();
        IEnumerable<Admin> GetKnownAdmins(string userId);
        void AddSetupAdmin(SetUpAdmin setUpAdmin);
        void RemoveSetupAdmin(int userId, int setupId);
    }
}