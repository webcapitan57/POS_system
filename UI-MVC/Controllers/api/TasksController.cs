using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.task;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/Tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ISetupService _setupService;
        private readonly ITaskService _taskService;

        public TasksController(ISetupService setupService, ITaskService taskService)
        {
            _setupService = setupService;
            _taskService = taskService;
        }


        [HttpGet("{taskId}")]
        public IActionResult GetTask(int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            return Ok(task.Questions.ToList());
        }


        [HttpPost("{taskId}/PhotoQuestions")]
        public IActionResult GetPhotoQuestion(int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            var photoQuestion = _taskService.AddPhotoQuestion(task);
            return Ok(photoQuestion);
        }

        [HttpPost("{taskId}/PhotoQuestions/{photoQuestionId}/SideQuestions")]
        public IActionResult GetSideQuestion(int taskId, int photoQuestionId)
        {
            var task = _taskService.ReadTaskById(taskId);
            var photoQuestion = task.Questions.First(pq => pq.PhotoQuestionId == photoQuestionId);
            var sideQuestion = _taskService.AddSideQuestion(photoQuestion);
            return Ok(sideQuestion);
        }

        [HttpPost("{taskId}/PhotoQuestions/{photoQuestionId}/SideQuestions/{sideQuestionId}/options")]
        public IActionResult GetOptions(int taskId, int photoQuestionId, int sideQuestionId)
        {
            var task = _taskService.ReadTaskById(taskId);
            var photoQuestion = task.Questions.First(pq => pq.PhotoQuestionId == photoQuestionId);
            var sideQuestion = photoQuestion.SideQuestions.First(sq => sq.SideQuestionId == sideQuestionId);
            var option = _taskService.AddSideQuestionOption(sideQuestion);
            return Ok(option);
        }

        [HttpDelete("{taskId}")]
        public IActionResult DeleteSetTask(int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            _taskService.DeleteTask(task);
            return NoContent();
        }

        [HttpDelete("{taskId}/PhotoQuestions/{photoQuestionId}")]
        public IActionResult DeletePhotoQuestion(int taskId, int photoQuestionId)
        {
            _taskService.DeletePhotoQuestion(photoQuestionId);
            return NoContent();
        }

        [HttpDelete("{taskId}/PhotoQuestions/{photoQuestionId}/SideQuestions/{sideQuestionId}")]
        public IActionResult DeleteSideQuestion(int taskId, int photoQuestionId, int sideQuestionId)
        {
            _taskService.DeleteSideQuestion(sideQuestionId);
            return NoContent();
        }

        [HttpDelete("{taskId}/PhotoQuestions/{photoQuestionId}/SideQuestions/{sideQuestionId}/Options")]
        public IActionResult DeleteOptions(int taskId, int photoQuestionId, int sideQuestionId)
        {
            _taskService.DeleteSideQuestionOption(sideQuestionId);
            return NoContent();
        }

        [HttpPut("Update/{taskId}")]
        public IActionResult UpdateSetTask(int taskId, [FromBody] TaskDTO taskDto)
        {
            Task task;
            if (taskDto.SetUpId != -1)
            {
                task = new SetTask()
                {
                    TaskId = taskId,
                    Title = taskDto.Title,
                    Info = taskDto.Info,
                    Questions = new List<PhotoQuestion>(),
                };
            }
            else if (taskDto.TeacherId != -1)
            {
                task = new TeacherTask()
                {
                    TaskId = taskId,
                    Title = taskDto.Title,
                    Info = taskDto.Info,
                    Questions = new List<PhotoQuestion>()
                };
            }
            else
            {
                throw new Exception();
            }

            foreach (var photoQuestionDto in taskDto.Questions)
            {
                PhotoQuestion photoQuestion = new PhotoQuestion()
                {
                    PhotoQuestionId = photoQuestionDto.PhotoQuestionId,
                    Tips = photoQuestionDto.Tips,
                    Question = photoQuestionDto.Question,
                    SideQuestions = new List<SideQuestion>(),
                    OnlyLocations = photoQuestionDto.OnlyLocations,
                    Locations = new List<Location>()
                };

                foreach (var sideQuestionDto in photoQuestionDto.SideQuestions)
                {
                    SideQuestion sideQuestion = new SideQuestion()
                    {
                        SideQuestionId = sideQuestionDto.SideQuestionId,
                        Question = sideQuestionDto.Question,
                        SideQuestionOptions = new List<SideQuestionOption>()
                    };
                    foreach (var sideQuestionOptionDto in sideQuestionDto.SideQuestionOptions)
                    {
                        SideQuestionOption sideQuestionOption = new SideQuestionOption()
                        {
                            SideQuestionOptionId = sideQuestionOptionDto.SideQuestionOptionId,
                            Value = sideQuestionOptionDto.Value
                        };
                        sideQuestion.SideQuestionOptions.Add(sideQuestionOption);
                    }

                    photoQuestion.SideQuestions.Add(sideQuestion);
                }

                task.Questions.Add(photoQuestion);
            }

            _taskService.UpdateTask(task);


            return Ok();
        }

        /*
        [HttpDelete("{taskId}/{photoQuestionId}/{sideQuestionId}/{sideQuestionOptionId}")]
        public IActionResult DeleteSideQuestionOption(int taskId, int photoQuestionId, int sideQuestionId, int sideQuestionOptionId)
        {
            var task = _taskService.ReadTaskById(taskId);
            _taskService.DeleteSideQuestion(task,photoQuestionId,sideQuestionId);
            return NoContent();
        }*/

        [HttpGet("{taskId}/PhotoQuestions")]
        public IActionResult GetAllPhotoQuestions(int taskId)
        {
            var task = _taskService.ReadTaskById(taskId);
            return Ok(task.Questions);
        }

        [HttpGet("{taskId}/PhotoQuestions/{photoQuestionId}/Locations/{locationId}")]
        public IActionResult GetLocation(int taskId, int photoQuestionId, int locationId)
        {
            var location = _taskService.GetLocation(locationId);
            return Ok(location);
        }

        [HttpGet("{taskId}/PhotoQuestions/{photoQuestionId}/Locations")]
        public IActionResult GetLocationByPhotoQuestion(int taskId, int photoQuestionId)
        {
            var task = _taskService.GetTask(taskId);
            var question = task.Questions.SingleOrDefault(pq => pq.PhotoQuestionId == photoQuestionId);

            return Ok(question.Locations);
        }

        [HttpPost("{taskId}/PhotoQuestions/{photoQuestionId}/Locations")]
        public IActionResult AddLocation(int taskId, int photoQuestionId, [FromBody] LocationDTO locationDto)
        {
            var task = _taskService.GetTask(taskId);
            var question = task.Questions.SingleOrDefault(pq => pq.PhotoQuestionId == photoQuestionId);

            var location = new Location()
            {
                Latitude = locationDto.Latitude,
                Longitude = locationDto.Longitude,
                Radius = locationDto.Radius,
                PhotoQuestion = question
            };

            _taskService.AddLocation(location);
            return Ok(location);
        }

        [HttpPut("{taskId}/PhotoQuestions/{photoQuestionId}/Locations/{locationId}")]
        public IActionResult UpdateLocation(int taskId, int photoQuestionId, int locationId,
            [FromBody] LocationDTO locationDto)
        {
            var task = _taskService.GetTask(taskId);
            var question = task.Questions.SingleOrDefault(pq => pq.PhotoQuestionId == photoQuestionId);

            var location = _taskService.GetLocation(locationId);
            location.Latitude = locationDto.Latitude;
            location.Longitude = locationDto.Longitude;
            location.Radius = locationDto.Radius;
            location.PhotoQuestion = question;

            _taskService.UpdateLocation(location);
            return Ok();
        }

        [HttpDelete("{taskId}/PhotoQuestions/{photoQuestionId}/Locations/{locationId}")]
        public IActionResult RemoveLocation(int taskId, int photoQuestionId, int locationId)
        {
            _taskService.DeleteLocation(locationId);
            return NoContent();
        }
    }
}