using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/Tags")]
    public class TagsController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public TagsController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpGet("{answerId:int}/{key}")]
        public IActionResult GetTagsOfAnswer(int answerId, string key)
        {
            var tags = _deliveryService.GetTagsOfAnswer(answerId);
            var correctTag = tags.FirstOrDefault(t => t.Description == key) ?? new Tag()
            {
                Value = ""
            };

            return Ok(correctTag);
        }
        [HttpDelete("{answerId}/{value}")]
        public IActionResult RemoveTag(int answerId,string value)
        {
            _deliveryService.RemoveTag(answerId, value);
            return NoContent();
        }
    }
}