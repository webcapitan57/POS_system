using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using UI_MVC.Models.Task;

namespace UI_MVC.Models.VSet
{
    public class VSetViewModel
    {
        public Group Group { get; set; }
        public IList<Answer> Answers { get; set; }
        public IList<Tag> Filters { get; set; }

        public IList<SetTask> SetTasks { get; set; }
        public int Page { get; set; }
        public int CurrentTaskId { get; set; }
        public bool IsLastPage { get; set; }
        public List<Answer> LikedAnswers { get; set; }
        public List<Answer> ReportedAnswers { get; set; }
        public User User { get; set; }
        public SetUp Setup { get; set; }
    }
}