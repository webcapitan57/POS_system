using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;

namespace POC.BL.Domain.task
{
    public class TeacherTask : Task
    {
        public Teacher Teacher { get; set; }
        public ICollection<TeacherTaskDelivery> TeacherTaskDeliveries { get; set; }
    }
}