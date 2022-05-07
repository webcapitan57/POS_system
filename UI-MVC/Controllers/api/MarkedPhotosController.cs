using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/markedphotos")]
    public class MarkedPhotosController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public MarkedPhotosController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPut("{answerId}/unmark")]
        public IActionResult UnmarkPhoto(int answerId)
        {
            _deliveryService.UnmarkAnswer(answerId);
            return Ok();
        }

        [HttpDelete("{answerId}")]
        public IActionResult RemoveMarkedPhoto(int answerId)
        {
            _deliveryService.RemoveAnswer(answerId);
            
            return Ok();
        }
    }
}