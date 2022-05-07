using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.setup;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/Admins")]
    public class AdminsController : ControllerBase
    {
        private readonly ISetupService _setupService;
        private readonly IAdminService _adminService;

        public AdminsController(ISetupService setupService, IAdminService adminService)
        {
            _setupService = setupService;
            _adminService = adminService;
        }

        [HttpPost("{userId}/{setupId}")]
        public IActionResult Post(int userId,int setupId)
        {
            var setup = _setupService.ReadSimpleSetupById(setupId);
            var admin = _adminService.GetAdmin(userId);

            _adminService.AddSetupAdmin(new SetUpAdmin()
            {
                SetUp = setup,
                Admin = admin
            });

            return Ok();

        }

        [HttpDelete("{userId}/{setupId}")]
        public IActionResult Delete(int userId,int setupId)
        {
            _adminService.RemoveSetupAdmin(userId, setupId);

            return Ok();

        }
    }
}