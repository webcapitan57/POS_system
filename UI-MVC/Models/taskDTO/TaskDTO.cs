using System.Collections.Generic;
using POC.BL.Domain.task;

namespace UI_MVC.Models
{
    public class TaskDTO
    {
        public int SetUpId { get; set; }
        public int TeacherId { get; set; }
        public string Title { get; set; }
        public string Info { get; set; }
        public List<PhotoQuestionDTO> Questions { get; set; }
    }
}