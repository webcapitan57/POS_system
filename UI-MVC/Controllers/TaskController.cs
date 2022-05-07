using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.SignalR;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using POC.Sockets;
using UI_MVC.Models;
using UI_MVC.Models.Task;
using Task = System.Threading.Tasks.Task;

namespace UI_MVC.Controllers
{
    public class TaskController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly IDeliveryService _deliveryService;
        private readonly ISetupService _setupService;
        private readonly GcloudStorage _storage;
        private readonly GcloudOptions _options;


        private readonly IHubContext<ModerateHub> _moderateHubContext;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public TaskController(IGroupService groupService, IStudentService studentService,
            IDeliveryService deliveryService, ISetupService setupService, IHubContext<ModerateHub> moderateHubContext,
            IWebHostEnvironment webHostEnvironment, GcloudStorage storage, IOptions<GcloudOptions> options)
        {
            _groupService = groupService;
            _studentService = studentService;
            _deliveryService = deliveryService;
            _setupService = setupService;
            _storage = storage;
            _options = options.Value;
            _moderateHubContext = moderateHubContext;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Show(string groupCode, int studentId)
        {
            var group = _groupService.GetGroupForShowAction(groupCode.ToUpper());
            var student = _studentService.GetStudentWithTags(studentId);

            var tags = new List<Tag>();

            tags.AddRange(group.GroupProfileAnswers
                .Select(groupProfileAnswer => new Tag()
                {
                    Description = groupProfileAnswer.AnsweredQuestion.Description,
                    Value = groupProfileAnswer.Value
                })
            );

            tags.AddRange(student.ProfileAnswers
                .Select(studentProfileAnswer => new Tag()
                {
                    Description = studentProfileAnswer.AnsweredQuestion.Description,
                    Value = studentProfileAnswer.Value
                })
            );

            var deliveries = _deliveryService.GetOrCreateDeliveries(group, tags, student);

            var model = new ShowViewModel()
            {
                Deliveries = deliveries,
                Group = group,
                SetUp = group.Teacher.SetUp
            };

            ViewBag.bName = _options.BucketName;
            return View(model);
        }

        public IActionResult ShowQuestions(int deliveryId, int setUpId)
        {
            //Get The delivery you need from the InMemory repository
            var taskDelivery = _deliveryService.GetDeliveryWithGroupStudentsAndTasks(deliveryId);
            var setup = _setupService.GetSetUp(setUpId);

            var model = new ShowQuestionsViewModel()
            {
                Delivery = taskDelivery,
                SetUp = setup
            };

            ViewBag.bName = _options.BucketName;
            return View(model);
        }

        public IActionResult ShowAnswers(int deliveryId, int photoQuestionId, int setUpId)
        {
            var taskDelivery = _deliveryService.GetDeliveryWithGroupStudentsAndTasks(deliveryId);
            var setup = _setupService.ReadSetupById(setUpId);


            var createAnswerViewModel = new CreateAnswerViewModel()
            {
                TaskDelivery = taskDelivery,
                SetUp = setup
            };

            createAnswerViewModel.PhotoQuestion = taskDelivery switch
            {
                SetTaskDelivery setTaskDelivery => setTaskDelivery.SetTask.Questions.FirstOrDefault(pq =>
                    pq.PhotoQuestionId == photoQuestionId),
                TeacherTaskDelivery teacherTaskDelivery => teacherTaskDelivery.TeacherTask.Questions.FirstOrDefault(
                    pq => pq.PhotoQuestionId == photoQuestionId),
                _ => createAnswerViewModel.PhotoQuestion
            };

            ViewBag.bName = _options.BucketName;
            return View(createAnswerViewModel);
        }

        [HttpGet]
        public IActionResult UploadPhoto(string previousAction, string groupCode, int studentId, int deliveryId,
            int photoQuestionId, int setUpId)
        {
            var deliveries = _deliveryService.GetDeliveriesOfStudentForUploadPhoto(studentId);
            var setup = _setupService.ReadSetupById(setUpId);

            var model = new PhotoViewModel()
            {
                Photos = deliveries.FirstOrDefault()?.SentPhotos.ToList(),
                GroupCode = groupCode,
                StudentId = studentId,
                DeliveryId = deliveryId,
                PhotoQuestionId = photoQuestionId,
                PreviousAction = previousAction,
                SetupId = setup.SetUpId,
                SetupName = setup.Name
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(PhotoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var deliveries = _deliveryService
                .GetDeliveriesOfStudentForUploadPhoto(model.StudentId).ToList();

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
            var filename = Guid.NewGuid() + "_" + model.Image.FileName;
            var filePath = Path.Combine(uploadsFolder, filename);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Image.CopyToAsync(fileStream);
            }

            _deliveryService.CreatePhoto(filename, deliveries);
            _storage.UploadImage(filePath, filename);
            model.Photos = _deliveryService.GetPhotosOfDelivery(deliveries.First().TaskDeliveryId).ToList();

            return View(model);
        }

        public IActionResult Finalize(string groupCode, int studentId, int setupId)
        {
            //Gets necessary objects
            var setup = _setupService.GetSetUp(setupId);
            var currentGroup = _groupService.GetGroupWithTeacher(groupCode);
            var taskDeliveries = _deliveryService.GetDeliveriesForCheckFromStudent(studentId).ToList();

            //checks if deliveries are complete
            var complete = true;
            foreach (var _ in taskDeliveries.Where(delivery => !_deliveryService.CheckDelivery(delivery)))
            {
                complete = false;
            }

            //Asks student's permission to submit delivery, if necessary
            if (!complete)
                return RedirectToAction("Check",
                    new {_studentId = studentId, groupCode = currentGroup.GroupCode, setupId = setup.SetUpId});

            //Shows 'Student filled task in completely.'
            return RedirectToAction("SendDeliveries",
                new {currentStudentId = studentId, groupCode = currentGroup.GroupCode});
        }

        public IActionResult Finished(int _studentId, int _setUpId)
        {
            ViewBag.image = "https://storage.googleapis.com/make_vrt_great_again/" + _storage.GetRandomGif("student");

            var setUp = _setupService.GetSetUp(_setUpId);
            //Deletes the student from the database.
            var student = _studentService.GetStudent(_studentId);
            _studentService.DeleteStudent(student);
            //_deliveryService.ClearStudentFromDeliveries(student);


            ViewBag.bName = _options.BucketName;
            //Shows view
            return View(setUp);
        }

        [HttpGet]
        public IActionResult Check(int _studentId, string groupCode, int setupId)
        {
            var taskDeliveries = _deliveryService.GetDeliveriesForCheckFromStudent(_studentId).ToList();
            var group = _groupService.GetGroup(groupCode);
            var setup = _setupService.GetSetUp(setupId);

            var model = new CheckViewModel()
            {
                Deliveries = taskDeliveries,
                Group = group,
                SetUp = setup
            };

            ViewBag.bName = _options.BucketName;
            return View(model);
        }
        
        [HttpPost]
        public IActionResult CheckConfirmed(int _currentStudentId, string _groupCode)
        {
            return RedirectToAction("SendDeliveries",
                new {currentStudentId = _currentStudentId, groupCode = _groupCode});
        }

        public async Task UploadDeliveries(Group group, IList<TaskDelivery> taskDeliveries)
        {
            List<string> photoList = new List<string>();
            //Submits every delivery
            foreach (var delivery in taskDeliveries)
            {
                if (_deliveryService.SaveDelivery(delivery))
                {
                    _groupService.UpdateTotalPhotosOfGroupInfo(group, delivery);
                    photoList.AddRange(delivery.SentPhotos.Select(p => p.Picture));
                }
            }

            photoList = photoList.Distinct().ToList();

            foreach (var photo in photoList)
            {
                if (await _storage.CheckForFace(photo))
                {
                    _deliveryService.SetFlaggedAnswers(photo);
                }
            }
        }

        
        public async Task<IActionResult> SendDeliveries(int currentStudentId, string groupCode)
        {
            // Get deliveries of student
            var taskDeliveries = _deliveryService.GetDeliveriesForCheckFromStudent(currentStudentId).ToList();
            var group = _groupService.GetGroupWithTeacher(groupCode);
            var currentSetup = group.Teacher.SetUp;

            //Saves all changes to the deliveries
            await UploadDeliveries(group, taskDeliveries);
            //SignalR

            //Get ids of deliveries
            var lastDeliveriesIds = taskDeliveries.Select(d => d.TaskDeliveryId);
            await _moderateHubContext.Clients.Groups(groupCode).SendAsync("SendDeliveries", lastDeliveriesIds);

            return RedirectToAction("Finished", new {_studentId = currentStudentId, _setUpId = currentSetup.SetUpId});
        }
    }
    
}