using System;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/qualityScoreEvents/{teacherId:int}/{answerId:int}")]
    public class QualityScoreEventsController : ControllerBase
    {
        public ITeacherService _teacherService { get; set; }
        public IDeliveryService _deliveryService { get; set; }
        
        public QualityScoreEventsController(ITeacherService teacherService, IDeliveryService deliveryService)
        {
            _teacherService = teacherService;
            _deliveryService = deliveryService;
        }

        [HttpPut("{score:int}")]
        public IActionResult AddEvent(int answerId, int teacherId, int score)
        {
            var answer = _deliveryService.GetAnswerById(answerId);
            var teacher = _teacherService.GetTeacher(teacherId);

            var eventType = score > 0 ? QualityScoreEventType.LIKED_BY_TEACHER : QualityScoreEventType.REPORTED_BY_TEACHER;
            
            var qualityScoreEvent = new QualityScoreEvent()
            {
                Answer = answer,
                Score = score,
                Teacher = teacher,
                EventType = eventType
            };

            _teacherService.AddQualityScoreEvent(qualityScoreEvent);
            _deliveryService.UpdateQualityScore(answer.AnswerId);
            
            return Ok();
        }
        
        [HttpDelete]
        public IActionResult RemoveEvent(int answerId, int teacherId)
        {
            _teacherService.RemoveQualityScoreEvent(teacherId, answerId);
            _deliveryService.UpdateQualityScore(answerId);
            
            return Ok();
        }
    }
}