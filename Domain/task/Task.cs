using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using POC.BL.Domain.user;

namespace POC.BL.Domain.task
{
    public abstract class Task
    {
        public int TaskId { get; set; }
        public string? Title { get; set; }
        public DateTime CreationDate { get; set; }
        public string? Info { get; set; }
        public ICollection<PhotoQuestion> Questions { get; set; }
        public ICollection<GroupTask> Groups { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is Task task)
            {
                return task.TaskId == TaskId;
            }

            return false;
        }
    }
}