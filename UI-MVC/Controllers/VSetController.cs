using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.delivery;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models.VSet;

namespace UI_MVC.Controllers
{
    //[Authorize(Policy = "GuestTeacherOrHigher")]
    public class VSetController : Controller
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IGroupService _groupService;
        private readonly ISetupService _setupService;
        private readonly GcloudOptions _options;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IUserService _userService;
        

        public VSetController(IDeliveryService deliveryService,
            IGroupService groupService, ISetupService setupService,
            IOptions<GcloudOptions> options, UserManager<UserAccount> userManager, IUserService userService)
        {
            _deliveryService = deliveryService;
            _groupService = groupService;
            _setupService = setupService;
            _options = options.Value;
            _userService = userService;
            _userManager = userManager;
        }
        
        public IActionResult Show(string groupCode, int setupId, int? currentTask, int page = 1)
        {
            const int answersPerPage = 50;
            var startIndex = (answersPerPage * (page - 1));
            var totalPages = 1;

            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var user = _userService.GetUserForSet(userAccount.Id);

            var group = _groupService.GetGroupWithTasks(groupCode);
            var setup = _setupService.GetSetUpWithSetTasks(setupId);

            var tasks = new List<Task>();
            if (group != null)
            {
                tasks.AddRange(group.Tasks.Select(t => t.Task).ToList());
            }
            else if (setup != null)
            {
                tasks.AddRange(setup.SetTasks);
            }

            var publishedDeliveries = new List<TaskDelivery>();
            var setTasks = new List<SetTask>();
            var answers = new List<Answer>();
            
            foreach (var task in tasks)
            {
                if (!(task is SetTask setTask)) continue;

                currentTask ??= setTask.TaskId;
                
                var allDeliveries = _deliveryService.GetSetDeliveriesOfSetTask(setTask).ToList();
                publishedDeliveries.AddRange(allDeliveries.Where(d => d.IsPublished));
                setTasks.Add(setTask);

                var currentAnswers = publishedDeliveries.SelectMany(a => a.Answers).Where(a => !a.Flagged).ToList()
                    .OrderByDescending(a => a.QualityScore).ToList();

                var total = Math.Ceiling((double) currentAnswers.Count / answersPerPage);
                if (total > totalPages)
                {
                    totalPages = (int) total;
                }

                if (!(currentAnswers.Count - startIndex < 0))
                {
                    answers.AddRange(currentAnswers.Count() >= startIndex + answersPerPage
                        ? currentAnswers.GetRange(startIndex, answersPerPage)
                        : currentAnswers.GetRange(startIndex, currentAnswers.Count - startIndex));
                }
            }

            var tags = publishedDeliveries.SelectMany(d => d.Tags).Distinct().ToList();

            Debug.Assert(currentTask != null, nameof(currentTask) + " != null");
            var model = new VSetViewModel()
            {
                Answers = answers.OrderByDescending(a => a.QualityScore).ToList(),
                Group = group,
                Setup = setup,
                User = user,
                Filters = tags,
                SetTasks = setTasks,
                Page = page,
                CurrentTaskId = (int) currentTask,
                IsLastPage = page == totalPages,
                LikedAnswers = user.QualityScoreEvents.Where(e => answers.Contains(e.Answer) && e.EventType == QualityScoreEventType.LIKED_BY_TEACHER).Select(e => e.Answer).ToList(),
                ReportedAnswers = user.QualityScoreEvents.Where(e => answers.Contains(e.Answer) && e.EventType == QualityScoreEventType.REPORTED_BY_TEACHER).Select(e => e.Answer).ToList()
            };
            ViewBag.bucketName = _options.BucketName;
            return View(model);
        }
    }
}