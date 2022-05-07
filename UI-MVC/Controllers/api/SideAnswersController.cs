using System.Collections.Generic;
using System.Linq;
using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using POC.BL.Domain.delivery;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    [ApiController]
    [Route("/api/SideAnswers")]
    
    public class SideAnswersController:ControllerBase
    {
        private readonly IDeliveryService _deliveryService;
        private readonly ITaskService _taskService;


        public SideAnswersController(IDeliveryService deliveryService,ITaskService taskService)
        {
            _deliveryService = deliveryService;
            _taskService = taskService;
        }
        
        [HttpPut("{deliveryId}")]
        public IActionResult UpdateSideAnswer(int deliveryId, [FromBody] List<UpdateSideAnswerDTO> sideAnswerDtos  )
        {
            List<SideAnswer> sideAnswersToUpdate = new List<SideAnswer>();

            foreach (var sideAnswer in sideAnswerDtos)
            {
                sideAnswersToUpdate.Add(new SideAnswer()
                {
                    SideAnswerId = sideAnswer.SideAnswerId,
                    GivenAnswer = sideAnswer.GivenAnswer,
                });
            }

            if (sideAnswersToUpdate.Count!=0)
            {
                _deliveryService.ChangeSideAnswers(sideAnswersToUpdate);    
            }
              
                
            return NoContent();
        }
    }
}