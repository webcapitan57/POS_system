using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo.InterFaces;

namespace POC.DAL.repo
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private PhotoAppDbContext _ctx;

        public DeliveryRepository(PhotoAppDbContext ctx)
        {
            _ctx = ctx;
        }

        public TaskDelivery ReadDeliveryWithStudentGroupAndTasksById(int id)
        {
            var delivery = _ctx.Deliveries.Find(id);

            switch (delivery)
            {
                case SetTaskDelivery _:
                    delivery = _ctx.SetTaskDeliveries
                        .Include(d => d.Group)
                        .Include(d => d.Student)
                        .Include(d => d.SetTask)
                        .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.TaskDelivery)
                        .Include(d => d.SetTask)
                        .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Locations)
                        .FirstOrDefault(d => d.TaskDeliveryId == id);
                    break;
                case TeacherTaskDelivery _:
                    delivery = _ctx.TeacherTaskDeliveries
                        .Include(d => d.Group)
                        .Include(d => d.Student)
                        .Include(d => d.TeacherTask)
                        .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Answers)
                        .ThenInclude(a => a.TaskDelivery)
                        .Include(d => d.TeacherTask)
                        .ThenInclude(t => t.Questions)
                        .ThenInclude(q => q.Locations)
                        .FirstOrDefault(d => d.TaskDeliveryId == id);
                    break;
            }

            return delivery;
        }

        public void UpdateDelivery(TaskDelivery delivery)
        {
            _ctx.Deliveries.Update(delivery);
            _ctx.SaveChanges();
        }

        public void RemoveDelivery(TaskDelivery delivery)
        {
            _ctx.Deliveries.Remove(delivery);
            _ctx.SaveChanges();
        }

        public IEnumerable<TaskDelivery> ReadDeliveriesOfGroup(string groupCode)
        {
            return _ctx.Deliveries
                .Include(d => d.Group)
                .Include(d => d.Tags)
                .Include(d => d.Answers)
                .ThenInclude(a => a.AssignedPhoto)
                .Include(d => d.Answers)
                .ThenInclude(a => a.AnsweredQuestion)
                .Include(d => d.Answers)
                .ThenInclude(a => a.CustomTags)
                .Include(d => d.Answers)
                .ThenInclude(a => a.SideAnswers)
                .ThenInclude(s => s.AnsweredQuestion)
                .Include(d => d.Answers)
                .Include(d => d.Answers)
                .Include(d => d.SentPhotos)
                .Where(delivery => delivery.Group.GroupCode.Equals(groupCode) && delivery.Finished);
        }

        public void CreateDelivery(TaskDelivery taskDelivery)
        {
            _ctx.Deliveries.Add(taskDelivery);
            _ctx.SaveChanges();
        }

        public IEnumerable<TaskDelivery> ReadDeliveriesWithSentPhotosTaskGroupTeacherAndStudentByStudentId(
            int studentId)
        {
            var deliveries = new List<TaskDelivery>();
            deliveries.AddRange(_ctx.SetTaskDeliveries
                .Include(d => d.Answers)
                .Include(d => d.SentPhotos)
                .Include(d => d.SetTask)
                .Include(d => d.Group)
                .ThenInclude(g => g.Teacher)
                .Include(d => d.Student)
                .Where(d => d.Student.StudentId == studentId));
            deliveries.AddRange(_ctx.TeacherTaskDeliveries
                .Include(d => d.Answers)
                .Include(d => d.SentPhotos)
                .Include(d => d.TeacherTask)
                .Include(d => d.Group)
                .ThenInclude(g => g.Teacher)
                .Include(d => d.Student)
                .Where(d => d.Student.StudentId == studentId));

            return deliveries.ToArray();
        }

        public IEnumerable<Photo> ReadPhotosOfDelivery(int deliveryId)
        {
            return _ctx.Photos
                .Include(p => p.Answers)
                .Include(p => p.TaskDelivery)
                .Where(p => p.TaskDelivery.TaskDeliveryId == deliveryId);
        }

        public Photo ReadPhotoById(int photoId)
        {
            return _ctx.Photos.Find(photoId);
        }

        public TaskDelivery ReadDeliveryWithAnswersById(int deliveryId)
        {
            return _ctx.Deliveries.Include(d => d.Answers)
                .SingleOrDefault(d => d.TaskDeliveryId == deliveryId);
        }

        public TaskDelivery ReadDeliveryWithAnswersAndSideAnswersById(int deliveryId)
        {
            return _ctx.Deliveries
                .Include(d => d.Tags)
                .Include(d => d.Answers)
                .ThenInclude(a => a.AssignedPhoto)
                .Include(d => d.Answers)
                .ThenInclude(a => a.SideAnswers)
                .ThenInclude(sa => sa.AnsweredQuestion)
                .Include(d => d.Answers)
                .ThenInclude(a => a.AnsweredQuestion)
                .SingleOrDefault(d => d.TaskDeliveryId == deliveryId);
        }

        public void CreateAnswer(Answer answer)
        {
            _ctx.Answers.Add(answer);
            _ctx.Photos.Update(answer.AssignedPhoto);
            _ctx.Deliveries.Update(answer.TaskDelivery);
            _ctx.SaveChanges();
        }

        public Answer ReadAnswerWithPhotoQuestionById(int deliveryId, int photoQuestionId, int photoId)
        {
            return _ctx.Answers.Include(a => a.AnsweredQuestion)
                .SingleOrDefault(a => a.TaskDelivery.TaskDeliveryId == deliveryId
                                      && a.AnsweredQuestion.PhotoQuestionId == photoQuestionId
                                      && a.AssignedPhoto.PhotoId == photoId);
        }

        public IEnumerable<SideAnswer> ReadSideAnswersOfAnswer(int answerId)
        {
            return _ctx.SideAnswers
                .Include(sa => sa.AnsweredQuestion)
                .ThenInclude(sq => sq.SideQuestionOptions)
                .Where(sa => sa.Answer.AnswerId == answerId);
        }

        public void UpdateSideAnswers(List<SideAnswer> sideAnswers)
        {
            _ctx.SideAnswers.UpdateRange(sideAnswers);
            _ctx.SaveChanges();
        }

        public IEnumerable<Answer> ReadAnswersOfPhotoQuestionWithPhotosById(int deliveryId, int photoQuestionId)
        {
            return _ctx.Answers
                .Include(a => a.AssignedPhoto)
                .Where(a => a.AnsweredQuestion.PhotoQuestionId == photoQuestionId
                            && a.TaskDelivery.TaskDeliveryId == deliveryId);
        }

        public void RemoveAnswer(int answerId)
        {
            var answerToRemove = GetAnswer(answerId);
            _ctx.Remove(answerToRemove);
            _ctx.SaveChanges();
        }

        private Answer GetAnswer(int answerId)
        {
            return _ctx.Answers.Find(answerId);
        }

        public IEnumerable<Photo> GetPhotosWithAnswers(string photoPicture)
        {
            return _ctx.Photos
                .Include(p => p.Answers)
                .Where(p => p.Picture.Equals(photoPicture));
        }
        
        public IEnumerable<TaskDelivery> ReadDeliveryForCheck(int studentId)
        {
            var taskDeliveries = new List<TaskDelivery>();

            taskDeliveries.AddRange(_ctx.SetTaskDeliveries
                .Include(d => d.Tags)
                .Include(d => d.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(d => d.SentPhotos)
                .Include(d => d.Student)
                .Include(d => d.SetTask)
                .ThenInclude(st => st.Questions)
                .Include(d => d.Answers)
                .ThenInclude(a => a.SideAnswers)
                .Where(d => d.Student.StudentId == studentId));

            taskDeliveries.AddRange(_ctx.TeacherTaskDeliveries
                .Include(d => d.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .ThenInclude(s => s.Logo)
                .Include(d => d.SentPhotos)
                .Include(d => d.Student)
                .Include(d => d.TeacherTask)
                .ThenInclude(tt => tt.Questions)
                .Include(d => d.Answers)
                .ThenInclude(a => a.SideAnswers)
                .Where(d => d.Student.StudentId == studentId));

            return taskDeliveries;
        }

        public void RemoveSideAnswer(SideAnswer sideAnswer)
        {
            _ctx.SideAnswers.Remove(sideAnswer);
            _ctx.SaveChanges();
        }

        public void CreateSideAnswer(SideAnswer sideAnswer)
        {
            _ctx.SideAnswers.Add(sideAnswer);
            _ctx.SaveChanges();
        }

        public Answer ReadAnswerById(int answerId)
        {
            return _ctx.Answers.Find(answerId);
        }

        public Answer ReadAnswerWithDeliveryAndTagsById(int answerId)
        {
            return _ctx.Answers
                .Include(a => a.TaskDelivery)
                .ThenInclude(d => d.Tags)
                .FirstOrDefault(a => a.AnswerId == answerId);
        }

        public IEnumerable<TaskDelivery> ReadSetDeliveriesOfSetTask(SetTask task)
        {
            return _ctx.SetTaskDeliveries
                .Include(d => d.Answers)
                .ThenInclude(a => a.AnsweredQuestion)
                .Include(d => d.Answers)
                .ThenInclude(a => a.AssignedPhoto)
                .Include(d => d.Answers)
                .ThenInclude(a => a.SideAnswers)
                .ThenInclude(s => s.AnsweredQuestion)
                .Include(d => d.Group)
                .ThenInclude(g => g.Teacher)
                .ThenInclude(t => t.SetUp)
                .Include(d => d.Tags)
                .Where(d => d.SetTask.TaskId == task.TaskId).ToList();
        }

        public Answer GetAnswerWithQualityScoreEvents(int answerId)
        {
            return _ctx.Answers
                .Include(a => a.QualityScoreEvents)
                .FirstOrDefault(a => a.AnswerId == answerId);
        }

        public void UpdateAnswer(Answer answer)
        {
            _ctx.Answers.Update(answer);
            _ctx.SaveChanges();
        }

        public List<Answer> GetAnswers(string picture)
        {
            return _ctx.Answers.Where(a => a.AssignedPhoto.Picture.Equals(picture)).ToList();
        }

        public void UpdateAnswers(IEnumerable<Answer> answers)
        {
            _ctx.Answers.UpdateRange(answers);
            _ctx.SaveChanges();
        }

        public IEnumerable<Answer> ReadMarkedAnswersOfSetUp(SetUp adminSetUp)
        {
            return _ctx.Answers
                .Include(a => a.AssignedPhoto)
                .Include(a => a.AnsweredQuestion)
                .Include(a => a.SideAnswers)
                .Where(a => a.TaskDelivery.Group.Teacher.SetUp == adminSetUp
                            && a.IsQuarantined);
        }

        public void DeleteAnswer(int answerId)
        {
            var answerToDelete = ReadAnswerById(answerId);
            _ctx.Answers.Remove(answerToDelete);
            _ctx.SaveChanges();
        }

        public Answer ReadAnswerWithTagsById(int answerId)
        {
            return _ctx.Answers
                .Include(a => a.CustomTags)
                .ThenInclude(t => t.Answer)
                .FirstOrDefault(a => a.AnswerId == answerId);
        }

        public void DeleteTag(int answerId, string value)
        {
            var tagToRemove = GetTag(answerId, value);
            _ctx.Tags.Remove(tagToRemove);
            _ctx.SaveChanges();
        }

        private Tag GetTag(int answerId, string value)
        {
            return _ctx.Tags.SingleOrDefault(t =>
                t.Answer.AnswerId == answerId && t.Value.ToLower().Equals(value.ToLower()));
        }
        
        public void RemoveAnswer(int photoQuestionId, int photoId)
        {
            var answerToRemove = GetAnswer(photoQuestionId, photoId);
            _ctx.Answers.Remove(answerToRemove);
            _ctx.SaveChanges();
        }

        private Answer GetAnswer(int photoQuestionId, int photoId)
        {
            return _ctx.Answers.FirstOrDefault(a => a.AnsweredQuestion.PhotoQuestionId == photoQuestionId &&
                                                    a.AssignedPhoto.PhotoId == photoId);
        }
    }
}