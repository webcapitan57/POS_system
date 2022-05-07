using Microsoft.AspNetCore.Mvc;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers
{
    [Route("/api/Present")]
    public class PresentController : ControllerBase
    {
        private IGroupService _groupService;

        public PresentController(IGroupService groupService)
        {
            _groupService = groupService;
        }
    }
}