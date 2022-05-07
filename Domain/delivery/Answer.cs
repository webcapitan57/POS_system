using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.Domain.delivery
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int QualityScore { get; set; }
        public bool Flagged { get; set; }
        public ICollection<Tag> CustomTags { get; set; } = new List<Tag>();

        public PhotoQuestion AnsweredQuestion { get; set; }

        [Required] public Photo AssignedPhoto { get; set; }
        public ICollection<SideAnswer> SideAnswers { get; set; }

        public TaskDelivery TaskDelivery { get; set; }
        public ICollection<QualityScoreEvent> QualityScoreEvents { get; set; }
        public bool IsQuarantined { get; set; }

        public void CreateSideAnswers()
        {
            var sideQuestions = AnsweredQuestion.SideQuestions;
            foreach (var sideQuestion in sideQuestions)
            {
                SideAnswers.Add(new SideAnswer()
                {
                    SideAnswerId = (byte) (SideAnswers.Count + 1),
                    Answer = this,
                    AnsweredQuestion = sideQuestion
                });
            }
        }
    }
}