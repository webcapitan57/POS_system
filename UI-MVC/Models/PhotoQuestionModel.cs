using POC.BL.Domain.task;

namespace UI_MVC.Models
{
    public class PhotoQuestionModel
    {
        public PhotoQuestion PhotoQuestion { get; set; }
        private int taskId;
        public PhotoQuestionModel(PhotoQuestion question)
        {
            PhotoQuestion = question;
        }
    }
}