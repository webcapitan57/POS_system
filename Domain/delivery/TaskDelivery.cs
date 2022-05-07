using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.Domain.delivery
{
    public abstract class TaskDelivery
    {
        public int TaskDeliveryId { get; set; }
        public Group Group { get; set; }
        public bool Finished { get; set; } = false;
        public IList<Photo> SentPhotos { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public IList<Tag> Tags { get; set; }
        public bool IsPublished { get; set; } = false;
        public Student? Student { get; set; }

        public abstract ICollection<PhotoQuestion> GetUnansweredPhotoQuestions();
    }
}