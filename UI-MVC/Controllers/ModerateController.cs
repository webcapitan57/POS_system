using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models.Moderate;

namespace UI_MVC.Controllers
{
    // [Authorize(Policy = "GuestTeacherOrHigher")]
    public class ModerateController : Controller
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IGroupService _groupService;
        private readonly ITeacherService _teacherService;
        private readonly GcloudOptions _options;
        private readonly UserManager<UserAccount> _userManager;
        private readonly ISetupService _setupService;
        
        public ModerateController(ITeacherService teacherService, IGroupService groupService,
            IDeliveryService deliveryService, IOptions<GcloudOptions> options, UserManager<UserAccount> userManager, ISetupService setupService)
        {
            _groupService = groupService;
            _deliveryService = deliveryService;
            _teacherService = teacherService;
            _options = options.Value;
            _userManager = userManager;
            _setupService = setupService;
        }

        [HttpGet("Moderate/{groupCode}")]
        public IActionResult ModerateDeliveries(string groupCode)
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherForSet(userAccount.Id);


            var deliveries = _deliveryService.GetDeliveriesOfGroup(groupCode).Where(d => !d.IsPublished).ToList();
            var group = _groupService.GetGroupWithTeacher(groupCode);
            var tags = new List<Tag>();
            var sQuestions = new List<StudentProfileQuestion>();
            if (deliveries.Count != 0)
            {
                tags.AddRange(deliveries.SelectMany(d => d.Tags).Distinct().ToList());
            }
            else
            {
                sQuestions = _setupService.GetSetUpStudentProfileQuestions(teacher.SetUp.SetUpId);
            }


            var model = new ModerateDeliveriesViewModel()
            {
                Deliveries = deliveries,
                Group = group,
                Teacher = teacher,
                Filters = tags,
                LikedAnswers = teacher.QualityScoreEvents
                    .Where(e => deliveries.SelectMany(d => d.Answers).Contains(e.Answer)
                                && e.EventType == QualityScoreEventType.LIKED_BY_TEACHER)
                    .Select(e => e.Answer).ToList(),
                StudentProfileQuestions = sQuestions
            };

            ViewBag.bucketName = _options.BucketName;
            return View(model);
        }


        [Route("/Present/Grid/{groupCode}")]
        public IActionResult ShowDeliveriesGrid(string groupCode)
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherForSet(userAccount.Id);

            var deliveries = _deliveryService.GetDeliveriesOfGroup(groupCode).ToList();

            var group = _groupService.GetGroupWithTeacher(groupCode);
            var answers = new List<Answer>();
            foreach (var delivery in deliveries)
            {
                answers.AddRange(delivery.Answers.Where(a => !a.Flagged));
            }

            //Creates and fills a list of rows for the html page
            var rows = new List<List<Answer>>();
            var i = 1;
            foreach (var answer in answers)
            {
                if (i == 1)
                {
                    rows.Add(new List<Answer>());
                }

                rows.Last().Add(answer);

                i += 1;
                if (i > 5)
                {
                    i = 1;
                }
            }

            var tags = deliveries.SelectMany(d => d.Tags).Distinct().ToList();

            var model = new ShowDeliveriesViewModel()
            {
                Answers = answers,
                Rows = rows,
                Group = group,
                Teacher = teacher,
                Filters = tags,
                LikedAnswers = teacher.QualityScoreEvents
                    .Where(e => answers.Contains(e.Answer)
                                && e.EventType == QualityScoreEventType.LIKED_BY_TEACHER)
                    .Select(e => e.Answer).ToList(),
            };
            ViewBag.bucketName = _options.BucketName;
            return View(model);
        }

        [Route("/Present/Slideshow/{groupCode}")]
        public IActionResult ShowDeliveriesSlide(string groupCode)
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherForSet(userAccount.Id);


            var deliveries = _deliveryService.GetDeliveriesOfGroup(groupCode).ToList();
            var group = _groupService.GetGroupWithTeacher(groupCode);
            var answers = new List<Answer>();
            foreach (var delivery in deliveries)
            {
                answers.AddRange(delivery.Answers.Where(a => !a.Flagged));
            }

            var tags = deliveries.SelectMany(d => d.Tags).Distinct().ToList();

            var model = new ShowDeliveriesViewModel()
            {
                Answers = answers,
                Group = group,
                Teacher = teacher,
                Filters = tags,
                LikedAnswers = teacher.QualityScoreEvents
                    .Where(e => answers.Contains(e.Answer)
                                && e.EventType == QualityScoreEventType.LIKED_BY_TEACHER)
                    .Select(e => e.Answer).ToList()
            };
            ViewBag.bucketName = _options.BucketName;
            return View(model);
        }

        [Route("/Present/Gallery/{groupCode}")]
        public IActionResult ShowDeliveriesGallery(string groupCode)
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherForSet(userAccount.Id);

            var deliveries = _deliveryService.GetDeliveriesOfGroup(groupCode).ToList();
            foreach (var delivery in deliveries)
            {
                delivery.IsPublished = true;
                _deliveryService.UpdateDelivery(delivery);
            }

            var group = _groupService.GetGroupWithTeacher(groupCode);
            group.AcceptDeliveries = false;
            group.Active = false;
            _groupService.UpdateGroup(group);

            var answers = new List<Answer>();
            foreach (var delivery in deliveries)
            {
                answers.AddRange(delivery.Answers.Where(a => !a.Flagged));
            }

            var tags = deliveries.SelectMany(d => d.Tags).Distinct().ToList();

            var model = new ShowDeliveriesViewModel()
            {
                Group = group,
                Teacher = teacher,
                Filters = tags,
                Answers = answers,
                LikedAnswers = teacher.QualityScoreEvents
                    .Where(e => answers.Contains(e.Answer)
                                && e.EventType == QualityScoreEventType.LIKED_BY_TEACHER)
                    .Select(e => e.Answer).ToList(),
            };
            ViewBag.bucketName = _options.BucketName;
            return View(model);
        }
    }
}