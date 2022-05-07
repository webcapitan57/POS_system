using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class CreateAnswerViewModel
    {
        public TaskDelivery TaskDelivery { get; set; }
        public PhotoQuestion PhotoQuestion { get; set; }
        public Answer? SelectedAnswer { get; set; }

        public SetUp SetUp { get; set; }
        
        
    }
}