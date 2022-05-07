using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.task;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using POC.Sockets;
using UI_MVC.Gcloud;
using UI_MVC.Models.AndroidDto;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/Deliveries")]
    public class DeliveriesController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IStudentService _studentService;
        private readonly IGroupService _groupService;
        private readonly ITaskService _taskService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly GcloudStorage _storage;
        private readonly IHubContext<ModerateHub> _moderateHubContext;

        public DeliveriesController(IDeliveryService deliveryService, IStudentService studentService,
            IGroupService groupService, ITaskService taskService, IWebHostEnvironment webHostEnvironment,
            GcloudStorage storage, IHubContext<ModerateHub> moderateHubContext)
        {
            _deliveryService = deliveryService;
            _studentService = studentService;
            _groupService = groupService;
            _taskService = taskService;
            _webHostEnvironment = webHostEnvironment;
            _storage = storage;
            _moderateHubContext = moderateHubContext;
        }

        [HttpGet("{groupCode}")]
        public IActionResult GetAnswers(string groupCode)
        {
            var deliveries = _deliveryService.GetDeliveriesOfGroup(groupCode);
            var answers = new List<Answer>();
            foreach (var delivery in deliveries)
            {
                answers.AddRange(delivery.Answers);
            }

            return Ok(answers.AsEnumerable());
        }

        [HttpPost]
        public async Task<IActionResult> PushDelivery([FromBody] List<DeliveryDTO> deliveryDtos)
        {
            var group = _groupService.GetGroupWithTasksAndAnswers(deliveryDtos[0].GroupCode);
            var student = _studentService.GetStudentWithTags(deliveryDtos[0].StudentId);
            var tags = new List<Tag>();
            foreach (var tagDto in deliveryDtos[0].Tags)
            {
                tags.Add(new Tag()
                {
                    Description = tagDto.Description,
                    Value = tagDto.Value
                });
            }

            foreach (var profileAnswer in group.GroupProfileAnswers)
            {
                tags.Add(new Tag()
                {
                    Description = profileAnswer.AnsweredQuestion.Description,
                    Value = profileAnswer.Value
                });
            }

            bool notFull = true;
            var trueDeliveries = _deliveryService.GetOrCreateDeliveries(group, tags, student);
            List<string> photoList = new List<string>();
            foreach (var deliveryDto in deliveryDtos)
            {
                if (notFull)
                {
                    var task = _taskService.GetTask(deliveryDto.TaskId);
                    TaskDelivery delivery;
                    if (deliveryDto.Discriminator == "Teacher")
                    {
                        delivery = trueDeliveries.First(taskDelivery =>
                            taskDelivery.Group.GroupCode == deliveryDto.GroupCode
                            && taskDelivery.Student.StudentId == deliveryDto.StudentId
                            && ((TeacherTaskDelivery) taskDelivery).TeacherTask.TaskId == deliveryDto.TaskId);
                    }
                    else
                    {
                        delivery = trueDeliveries.First(taskDelivery =>
                            taskDelivery.Group.GroupCode.Equals(deliveryDto.GroupCode)
                            && taskDelivery.Student.StudentId == deliveryDto.StudentId
                            && ((SetTaskDelivery) taskDelivery).SetTask.TaskId == deliveryDto.TaskId);
                    }

                    delivery.Answers = new List<Answer>();
                    delivery.SentPhotos = new List<Photo>();

                    foreach (var answerDto in deliveryDto.Answers)
                    {
                        var photoQuestion = task.Questions.First(question =>
                            question.PhotoQuestionId == answerDto.AnsweredQuestionId);
                        byte[] bytes = Convert.FromBase64String(answerDto.Photo);
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        var fileName = Guid.NewGuid() + "test.jpg";
                        var filePath = Path.Combine(uploadsFolder, fileName);
                        System.IO.File.WriteAllBytes(filePath, bytes);
                        _storage.UploadImage(filePath, fileName);
                        System.IO.File.Delete(filePath);


                        var photo = new Photo()
                        {
                            Answers = new List<Answer>(),
                            TaskDelivery = delivery,
                            Picture = fileName //TODO fix this shit arno
                        };
                        delivery.SentPhotos.Add(photo);
                        var answer = new Answer()
                        {
                            AnsweredQuestion = photoQuestion,
                            AssignedPhoto = photo,
                            SideAnswers = new List<SideAnswer>(),
                            TaskDelivery = delivery
                        };
                        foreach (var sideAnswerDto in answerDto.SideAnswers)
                        {
                            answer.SideAnswers.Add(new SideAnswer()
                            {
                                Answer = answer,
                                AnsweredQuestion = photoQuestion.SideQuestions.First(question =>
                                    question.SideQuestionId == sideAnswerDto.AnsweredQuestionId),
                                GivenAnswer = sideAnswerDto.GivenAnswer
                            });
                        }

                        delivery.Answers.Add(answer);
                    }

                    notFull = _deliveryService.SaveDelivery(delivery);

                    if (notFull)
                    {
                        _groupService.UpdateTotalPhotosOfGroupInfo(group, delivery);
                        photoList.AddRange(delivery.SentPhotos.Select(p => p.Picture));
                    }
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

            if (notFull)
            {
                //Send the added deliveries to the moderate screen of the teacher by using signalR
                var lastDeliveriesIds = trueDeliveries.Select(d => d.TaskDeliveryId);
                await _moderateHubContext.Clients.Groups(group.GroupCode)
                    .SendAsync("SendDeliveries", lastDeliveriesIds);
            }

            Console.WriteLine("wait, we actually got here?" + notFull);
            return Ok(notFull);
        }
    }
}