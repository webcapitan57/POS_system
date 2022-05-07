using System;
using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;

namespace UI_MVC.Models
{
    public class EditTaskModel
    {
     
        public int TaskId { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public int SetUpId { get; set; }
        public int TeacherId { get; set; }
        public bool NewTask { get; set; }

        public SetUp Setup { get; set; }
        
    }
}