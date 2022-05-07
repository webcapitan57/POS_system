using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models;


namespace UI_MVC.Controllers
{
   // [Authorize(Policy = "TeacherOrHigher")]
   [Route("createTasks")]
    public class CreateTaskController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly ISetupService _setupService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly ITeacherService _teacherService;
        private readonly GcloudOptions _options;

        public CreateTaskController(ITaskService taskService, ISetupService setupService, UserManager<UserAccount> userManager, ITeacherService teacherService,IOptions<GcloudOptions> options)
        {
            _taskService = taskService;
            _setupService = setupService;
            _userManager = userManager;
            _teacherService = teacherService;
            _options = options.Value;
        }
        
        [HttpGet("CreateSetTask/{setUpId}")]
        public IActionResult CreateSetTask(int setUpId)
        {
            var setUp = _setupService.ReadSetupById(setUpId);
            var task = _taskService.AddSetTask(setUp);
            return Redirect("/createTasks/EditTask/" + task.TaskId);
        }
        
        public IActionResult CreateTeachTask()
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherWithGroupsAndTasks(userAccount.Id);
            var task = _taskService.AddTeacherTask(teacher);
            
            ViewBag.bucketName = _options.BucketName;
            return Redirect("/createTasks/EditTask/" + task.TaskId);
        }
        
        [HttpGet("EditTask/{taskId:int}")]
        public IActionResult EditTask(int taskId, bool newTask = false)
        {
             var task = _taskService.ReadTaskById(taskId);
             var editTaskModel = new EditTaskModel();

             switch (task)
             {
                 case SetTask setTask:
                     editTaskModel.Info = setTask.Info;
                     editTaskModel.Title = setTask.Title;
                     editTaskModel.TaskId = setTask.TaskId;
                     editTaskModel.SetUpId = setTask.SetUp.SetUpId;
                     editTaskModel.TeacherId = -1;
                     editTaskModel.NewTask = newTask;
                     editTaskModel.Setup = setTask.SetUp;
                     break;
                 case TeacherTask teacherTask:
                     editTaskModel.Info = teacherTask.Info;
                     editTaskModel.Title = teacherTask.Title;
                     editTaskModel.TaskId = teacherTask.TaskId;
                     editTaskModel.SetUpId = -1;
                     editTaskModel.TeacherId = teacherTask.Teacher.UserId;
                     editTaskModel.NewTask = newTask;
                     editTaskModel.Setup = teacherTask.Teacher.SetUp;
                     break;
             }
             ViewBag.bucketName = _options.BucketName;
             return View(editTaskModel);
        }
        
        [HttpPost]
        public IActionResult SaveSetTask(EditTaskModel model)
        {
            return RedirectToAction("Edit", "SetUp", new { Id = model.SetUpId });
        }
        
        [HttpPost]
        public IActionResult SaveTeacherTask(EditTaskModel model)
        {
            return RedirectToAction("Index", "Group");
        }
    }
}