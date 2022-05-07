using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class SelectedTaskViewModel
    {
        public List<TaskDelivery> TaskDeliveries { get; set; }
        public Group Group { get; set; }
        public Student Student { get; set; }
        public TaskDelivery CurrentTaskdelivery { get; set; }
        public PhotoQuestion CurrentQuestion { get; set; }
    }
}