using System.Collections.Generic;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.BL.logic.InterFaces
{
    public interface IDeliveryService
    {
        public Answer AddNewAnswer(int photoId, int photoQuestionId,int deliveryId);
        public bool CheckDelivery(TaskDelivery delivery);
        public IEnumerable<TaskDelivery> GetOrCreateDeliveries(Group @group, List<Tag> tags, Student student);
        public TaskDelivery GetDeliveryWithGroupStudentsAndTasks(int deliveryId);
        public TaskDelivery GetDeliveryWithAnswersAndSideAnswersById(int deliveryId);
        public IEnumerable<TaskDelivery> GetDeliveriesOfStudentForUploadPhoto(int studentId);
        public Photo GetPhoto(int photoId);
        public Photo CreatePhoto(string picture, IEnumerable<TaskDelivery> deliveries);
        public void ChangeSideAnswers(List<SideAnswer> sideAnswer);
        public IEnumerable<Photo> GetPhotosOfDelivery(int deliveryId);
        public Answer GetAnswerWithPhotoQuestion(int deliveryId, int photoQuestionId, int photoId);
        public IEnumerable<SideAnswer> GetSideAnswersOfAnswer(int answerAnswerId);
        public IEnumerable<Photo> GetAssignedPhotos(int deliveryId,int photoQuestionId);
        public void RemoveAnswer(int answerId);
        public IEnumerable<Photo> GetPhotosWithAnswers(string photoPicture);
        public void RemovePhotos(IEnumerable<Photo> photosList);
        public bool SaveDelivery(TaskDelivery delivery);
        public IEnumerable<TaskDelivery> GetDeliveriesForCheckFromStudent(int studentId);
        public IEnumerable<TaskDelivery> GetDeliveriesOfGroup(string groupCode);
        public Answer GetAnswerById(int answerId);
        public IEnumerable<Tag> GetTagsOfAnswer(int answerId);
        public IEnumerable<TaskDelivery> GetSetDeliveriesOfSetTask(SetTask task);
        public void UpdateQualityScore(int answerId);
        public void SetFlaggedAnswers(string photo);
        public IEnumerable<Answer> GetMarkedAnswers(List<SetUp> adminSetUps);
        public void UnmarkAnswer(int answerId);
        public void UpdateAnswer(Answer answer);
        public void UpdateDelivery(TaskDelivery delivery);
        public Answer GetAnswerWithCustomTagsById(int answerId);
        public void RemoveTag(int answerId,string value);
        public void RemoveAnswer(int photoQuestionId, int photoId);
    }
}