using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;

namespace POC.DAL.repo.InterFaces
{
    public interface IDeliveryRepository
    {
        public TaskDelivery ReadDeliveryWithStudentGroupAndTasksById(int id);
        public TaskDelivery ReadDeliveryWithAnswersAndSideAnswersById(int id);
        public void UpdateDelivery(TaskDelivery delivery);
        public void RemoveDelivery(TaskDelivery delivery);
        public IEnumerable<TaskDelivery> ReadDeliveriesOfGroup(string groupCode);
        public void CreateDelivery(TaskDelivery teacherTaskDelivery);
        public IEnumerable<TaskDelivery>ReadDeliveriesWithSentPhotosTaskGroupTeacherAndStudentByStudentId(int studentId);
        public IEnumerable<Photo> ReadPhotosOfDelivery(int deliveryId);
        public Photo ReadPhotoById(int photoId);
        public TaskDelivery ReadDeliveryWithAnswersById(int deliveryId);
        public void CreateAnswer(Answer answer);
        public Answer ReadAnswerWithPhotoQuestionById(int deliveryId, int photoQuestionId, int photoId);
        public IEnumerable<SideAnswer> ReadSideAnswersOfAnswer(int answerAnswerId);
        public void UpdateSideAnswers(List<SideAnswer> sideAnswer);
        public IEnumerable<Answer> ReadAnswersOfPhotoQuestionWithPhotosById(int deliveryId,int photoQuestionId);
        public void RemoveAnswer(int answer);
        public IEnumerable<Photo> GetPhotosWithAnswers(string photoPicture);
        public IEnumerable<TaskDelivery> ReadDeliveryForCheck(int studentId);
        public void RemoveSideAnswer(SideAnswer sideAnswer);
        public void CreateSideAnswer(SideAnswer sideAnswer);
        public Answer ReadAnswerById(int answerId);
        public Answer ReadAnswerWithDeliveryAndTagsById(int answerId);
        public IEnumerable<TaskDelivery> ReadSetDeliveriesOfSetTask(SetTask task);
        public Answer GetAnswerWithQualityScoreEvents(int answerId);
        public void UpdateAnswer(Answer answer); 
        public List<Answer> GetAnswers(string picture);
        public void UpdateAnswers(IEnumerable<Answer> answers);
        public IEnumerable<Answer> ReadMarkedAnswersOfSetUp(SetUp adminSetUps);
        public void DeleteAnswer(int answerId);
        public Answer ReadAnswerWithTagsById(int answerId);
        public void DeleteTag(int answerId,string value);
        public void RemoveAnswer(int photoQuestionId, int photoId);
    }
}