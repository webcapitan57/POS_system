using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.task;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/GroupTasks")]
    public class GroupTasksController: ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly IGroupService _groupService;

        public GroupTasksController(ITaskService taskService, IGroupService groupService)
        {
            _taskService = taskService;
            _groupService = groupService;
        }
        
        [HttpPost("{groupCode}/{taskId}")]
        public IActionResult Post(string groupCode,int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            var group = _groupService.GetGroup(groupCode);

            _groupService.AddGroupTask(new GroupTask
            {
                Task = task,
                Group = group
            });

            return Ok();

        }

        [HttpDelete("{groupCode}/{taskId}")]
        public IActionResult Delete(string groupCode,int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            var group = _groupService.GetGroup(groupCode);
            var groupTask = _groupService.GetGroupTask(task.TaskId, group.GroupId);
            _groupService.RemoveGroupTask(groupTask);

            return Ok();

        }
        
    }
}