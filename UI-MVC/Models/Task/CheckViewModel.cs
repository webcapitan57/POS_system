using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace UI_MVC.Models.Task
{
    public class CheckViewModel
    {
        public SetUp SetUp { get; set; }
        public IList<TaskDelivery> Deliveries { get; set; }
        public Group Group { get; set; }
    }
}