using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.task;

namespace POC.BL.Domain.delivery
{
    public class SetTaskDelivery : TaskDelivery
    {
        public SetTask SetTask { get; set; }

        public override ICollection<PhotoQuestion> GetUnansweredPhotoQuestions()
        {
            ICollection<PhotoQuestion> answeredQuestions =
                Answers.Select(answer => answer.AnsweredQuestion).Distinct().ToList();
            
            return SetTask.Questions.Where(question => !answeredQuestions.Contains(question)).ToList();
        }

        public override bool Equals(object obj)
        {
            try
            {
                var otherDelivery = obj as SetTaskDelivery;

                if (TaskDeliveryId == otherDelivery.TaskDeliveryId)
                {
                    return true;
                }
                
                return (Group.Equals(otherDelivery.Group) && Student.Equals(otherDelivery.Student) &&
                        SetTask.Equals(otherDelivery.SetTask));
            }
            catch
            {
                return false;
            }
        }
    }
}