using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/setups")]
    public class SetUpsController : ControllerBase
    {
        private readonly ISetupService _setupService;
        private readonly IAdminService _adminService;

        public SetUpsController(ISetupService setupService, IAdminService adminService)
        {
            _setupService = setupService;
            _adminService = adminService;
        }

        [HttpGet("{setUpId}")]
        public IActionResult GetSetUp(int setUpId)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            return Ok(setUp);
        }

        [HttpDelete("{setUpId}")]
        public IActionResult DeleteSetUp(int setUpId)
        {
            _setupService.RemoveSetUp(setUpId);
            return NoContent();
        }

        [HttpPut("{setUpId}")]
        public IActionResult UpdateSetUp(int setUpId, [FromBody] SetUpDTO setUpDto)
        {
            var setUp = _setupService.GetSetUp(setUpId);
            setUp.Name = setUpDto.Name;
            setUp.GeneralText = setUpDto.GeneralText;
            setUp.CreateTasks = setUpDto.CreateTasks;
            setUp.PrimColor = setUpDto.PrimColor;
            setUp.SecColor = setUpDto.SecColor;
            setUp.Archived = setUpDto.Archived;
            setUp.AllowLocations = setUpDto.AllowLocations;
            
            _setupService.UpdateSetUp(setUp);
            return RedirectToAction("Index", "SetUp");
        }
        
        [HttpPut("{setUpId}/archive")]
        public IActionResult ArchiveSetUp(int setUpId)
        {
            _setupService.ArchiveSetup(setUpId);
            var setUp = _setupService.GetSetUp(setUpId);
            return Ok(setUp.Archived);
        }
    }
}