using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class ShowViewModel
    {
        public SetUp SetUp { get; set; }
        public Group Group { get; set; }
        public IEnumerable<TaskDelivery> Deliveries { get; set; }
    }
}