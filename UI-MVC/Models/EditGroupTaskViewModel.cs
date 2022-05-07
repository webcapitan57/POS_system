using System.Collections.Generic;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace UI_MVC.Models
{
    public class EditGroupTaskViewModel
    {
        public Group CurrentGroup { get; set; }
        public ICollection<SetTask> SetTasks { get; set; }
        public ICollection<TeacherTask> TeacherTasks { get; set; }
        public List<POC.BL.Domain.task.Task> AlreadyAddedTasks { get; set; }
    }
}