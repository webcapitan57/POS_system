using System.Collections.Generic;
using System.Linq;
using Google.Apis.Storage.v1.Data;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers
{
    [ApiController]
    [Route("/api/Answers")]
    public class AnswersController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public AnswersController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("{deliveryId}/{photoQuestionId}/{photoId}")]
        public IActionResult GetAnswer(int deliveryId, int photoQuestionId, int photoId)
        {
            Answer answer = _deliveryService.GetAnswerWithPhotoQuestion(deliveryId, photoQuestionId, photoId);
            if (answer == null)
            {
                return NoContent();
            }

            List<SideAnswer> sideAnswers = _deliveryService.GetSideAnswersOfAnswer(answer.AnswerId).ToList();
            answer.SideAnswers = new List<SideAnswer>();
            answer.SideAnswers = sideAnswers;


            return Ok(answer);
        }
        
        [HttpGet("{deliveryId}")]
        public IActionResult GetDeliveryWithAnswersAndSideAnswers(int deliveryId)
        {
            var delivery = _deliveryService.GetDeliveryWithAnswersAndSideAnswersById(deliveryId);
            return Ok(delivery);
        }

        [HttpPost("{deliveryId}/{photoQuestionId}/{photoId}")]
        public IActionResult CreateAnswer(int deliveryId, int photoQuestionId, int photoId)
        {

           Answer answer = _deliveryService.AddNewAnswer(photoId, photoQuestionId, deliveryId);
           
            return Ok(answer);
        }
        
        [HttpDelete("{photoQuestionId}/{photoId}")]
        public IActionResult RemoveAnswer(int photoQuestionId, int photoId)
        {
            _deliveryService.RemoveAnswer(photoQuestionId, photoId);
            return Ok();
        }

        [HttpPut("{answerId:int}/flag")]
        public IActionResult FlagAnswer(int answerId)
        {
            var answer = _deliveryService.GetAnswerById(answerId);
            answer.Flagged = true;
            _deliveryService.UpdateAnswer(answer);
            return Ok();
        }
        
        [HttpPut("{answerId:int}/unflag")]
        public IActionResult UnflagAnswer(int answerId)
        {
            var answer = _deliveryService.GetAnswerById(answerId);
            answer.Flagged = false;
            _deliveryService.UpdateAnswer(answer);
            return Ok();
        }

        [HttpPut("{answerId:int}/customTags/add/{value}")]
        public IActionResult AddCustomTag(int answerId, string value)
        {
            var answer = _deliveryService.GetAnswerWithCustomTagsById(answerId);
            var tag = new Tag()
            {
                Answer = answer,
                Value = value,
                Description = "CustomTag"
            };
            answer.CustomTags.Add(tag);
            _deliveryService.UpdateAnswer(answer);
            return Ok();
        }
        
        

        
    }
}

        
