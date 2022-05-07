using System.Collections.Generic;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public Admin GetAdmin(int id)
        {
            return _adminRepository.ReadAdminById(id);
        }

        public Admin GetAdmin(string id)
        {
            return _adminRepository.ReadAdminById(id);
        }

        public IEnumerable<SetUp> GetSetUpsOfAdmin(string id)
        {
            return _adminRepository.ReadSetUpsOfAdmin(id);
        }

        public void UpdateAdmin(Admin admin)
        {
            _adminRepository.UpdateAdmin(admin);
        }

        public int AddAdmin()
        {
            var admin = new Admin();
            return _adminRepository.AddAdmin(admin);
        }

        public IEnumerable<Admin> GetKnownAdmins(string userId)
        {
            return _adminRepository.GetKnownAdmins(userId);
        }

        public void AddSetupAdmin(SetUpAdmin setUpAdmin)
        {
            _adminRepository.CreateSetupAdmin(setUpAdmin);
        }

        
        public void RemoveSetupAdmin(int userId, int setupId)
        {
            var setupAdmin = _adminRepository.GetSetupAdmin(userId, setupId);
            _adminRepository.RemoveSetupAdmin(setupAdmin);
        }
    }
}