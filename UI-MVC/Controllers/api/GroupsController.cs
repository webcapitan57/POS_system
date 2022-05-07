using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/Groups")]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupsController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet("{groupCode}")]
        public IActionResult Show(string groupCode)
        {
           var group = _groupService.GetGroupForShowAction(groupCode) ?? new Group()
           {
               GroupId = -1,
               GroupCode = "",
               AcceptDeliveries = false,
               Active = false,
               GroupProfileAnswers = new List<GroupProfileAnswer>(),
               HasLimit = false,
               Info = null,
               MaxParticipants = 0,
               Students = new List<Student>(),
               Tasks = new List<GroupTask>()
           };
           return Ok(group);
        }

        [HttpDelete("group/{groupCode}")]
        public IActionResult DeleteAnswers(string groupCode)
        {
            _groupService.RemoveAnswers(groupCode);
            return NoContent();
        }

        [HttpDelete("answer/{answerId}")]
        public IActionResult DeleteAnswer(int answerId)
        {
            _groupService.RemoveAnswer(answerId);
            return NoContent();
        }
    }
}