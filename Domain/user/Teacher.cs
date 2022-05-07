using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;

namespace POC.BL.Domain.user
{
    public class Teacher : User
    {
        
        
        public ICollection<TeacherTask> TeacherTasks { get; set; }

        public bool IsGuest { get; set; }
        public SetUp SetUp { get; set; }

        public ICollection<Group> Groups { get; set; }


    }
}