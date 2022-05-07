using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.delivery;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;

namespace POC.BL.logic
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly IPhotoRepository _photoRepository;
        
        private const int QuarantineTreshold = -5;

        public DeliveryService(IDeliveryRepository deliveryRepository,
            ITaskRepository taskRepository, IPhotoRepository photoRepository)
        {
            _deliveryRepository = deliveryRepository;
            _taskRepository = taskRepository;
            _photoRepository = photoRepository;
        }

        public Answer AddNewAnswer(int photoId, int photoQuestionId, int deliveryId)
        {
            var photo = _deliveryRepository.ReadPhotoById(photoId);
            var delivery = _deliveryRepository.ReadDeliveryWithAnswersById(deliveryId);
            var photoQuestion = _taskRepository.ReadPhotoQuestionById(photoQuestionId);
            var sideQuestions = photoQuestion.SideQuestions.ToList();

            var answer = new Answer()
            {
                QualityScoreEvents = new List<QualityScoreEvent>(),
                TaskDelivery = delivery,
                AssignedPhoto = photo,
                AnsweredQuestion = photoQuestion,
                QualityScore = 0,
                Flagged = false,
                SideAnswers = new List<SideAnswer>()
            };
            delivery.Answers ??= new List<Answer>();
            delivery.Answers.Add(answer);
            photo.Answers ??= new List<Answer>();
            photo.Answers.Add(answer);
            _deliveryRepository.CreateAnswer(answer);

            foreach (var t in sideQuestions)
            {
                var sideAnswer = new SideAnswer()
                {
                    Answer = answer,
                    AnsweredQuestion = t
                };

                t.SideAnswers ??= new List<SideAnswer>();
                t.SideAnswers.Add(sideAnswer);

                answer.SideAnswers.Add(sideAnswer);
                _deliveryRepository.CreateSideAnswer(sideAnswer);
            }

            _deliveryRepository.UpdateDelivery(delivery);
            return answer;
        }

        public bool CheckDelivery(TaskDelivery delivery)
        {
            var unansweredQuestions = delivery.GetUnansweredPhotoQuestions();
            return unansweredQuestions.Count == 0;
        }

        public IEnumerable<TaskDelivery> GetOrCreateDeliveries(Group @group, List<Tag> tags, Student student)
        {
            var tasks = group.Tasks;
            var taskDeliveries = new List<TaskDelivery>();
            var alreadyMadeDeliveries = _deliveryRepository
                .ReadDeliveriesWithSentPhotosTaskGroupTeacherAndStudentByStudentId(student.StudentId).ToList();
            student.Deliveries ??= new List<TaskDelivery>();

            foreach (var groupTask in tasks)
            {
                var task = groupTask.Task;

                switch (task)
                {
                    case TeacherTask teacherTask:
                    {
                        var teacherTaskDelivery = new TeacherTaskDelivery()
                        {
                            TeacherTask = teacherTask,
                            Group = group,
                            Answers = new List<Answer>(),
                            SentPhotos = new List<Photo>(),
                            Student = student,
                            Tags = new List<Tag>()
                        };
                        student.Deliveries.Add(teacherTaskDelivery);

                        foreach (var tag in tags)
                        {
                            var newTag = new Tag()
                            {
                                Description = tag.Description,
                                Value = tag.Value,
                                TaskDelivery = teacherTaskDelivery
                            };
                            teacherTaskDelivery.Tags.Add(newTag);
                        }

                        if (!alreadyMadeDeliveries.Contains(teacherTaskDelivery))
                        {
                            _deliveryRepository.CreateDelivery(teacherTaskDelivery);
                            taskDeliveries.Add(teacherTaskDelivery);
                        }
                        else
                        {
                            foreach (var taskDelivery in alreadyMadeDeliveries)
                            {
                                if (!(taskDelivery is TeacherTaskDelivery tDelivery)) continue;
                                if (tDelivery.Equals(teacherTaskDelivery))
                                {
                                    taskDeliveries.Add(tDelivery);
                                }
                            }
                        }

                        break;
                    }
                    case SetTask setTask:
                    {
                        var setTaskDelivery = new SetTaskDelivery()
                        {
                            SetTask = setTask,
                            Group = group,
                            Answers = new List<Answer>(),
                            SentPhotos = new List<Photo>(),
                            Student = student,
                            Tags = new List<Tag>()
                        };
                        student.Deliveries.Add(setTaskDelivery);

                        foreach (var tag in tags)
                        {
                            var newTag = new Tag()
                            {
                                Description = tag.Description,
                                Value = tag.Value,
                                TaskDelivery = setTaskDelivery
                            };
                            setTaskDelivery.Tags.Add(newTag);
                        }

                        if (!alreadyMadeDeliveries.Contains(setTaskDelivery))
                        {
                            _deliveryRepository.CreateDelivery(setTaskDelivery);
                            taskDeliveries.Add(setTaskDelivery);
                        }
                        else
                        {
                            foreach (var taskDelivery in alreadyMadeDeliveries)
                            {
                                if (!(taskDelivery is SetTaskDelivery sDelivery)) continue;
                                if (sDelivery.Equals(setTaskDelivery))
                                {
                                    taskDeliveries.Add(sDelivery);
                                }
                            }
                        }

                        break;
                    }
                }
            }

            return taskDeliveries;
        }

        public TaskDelivery GetDeliveryWithGroupStudentsAndTasks(int deliveryId)
        {
            return _deliveryRepository.ReadDeliveryWithStudentGroupAndTasksById(deliveryId);
        }

        public TaskDelivery GetDeliveryWithAnswersAndSideAnswersById(int deliveryId)
        {
            return _deliveryRepository.ReadDeliveryWithAnswersAndSideAnswersById(deliveryId);
        }

        public IEnumerable<TaskDelivery> GetDeliveriesOfStudentForUploadPhoto(int studentId)
        {
            return _deliveryRepository.ReadDeliveriesWithSentPhotosTaskGroupTeacherAndStudentByStudentId(studentId);
        }

        public Photo GetPhoto(int photoId)
        {
            return _deliveryRepository.ReadPhotoById(photoId);
        }

        public Photo CreatePhoto(string picture, IEnumerable<TaskDelivery> deliveries)
        {
            ICollection<Photo> photos = new List<Photo>();
            foreach (var t in deliveries)
            {
                var photoToAdd = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = t,
                    Picture = picture
                };
                t.SentPhotos.Add(photoToAdd);
                photos.Add(_photoRepository.AddPhoto(photoToAdd));
            }

            return photos.First();
        }

        public void ChangeSideAnswers(List<SideAnswer> sideAnswers)
        {
            _deliveryRepository.UpdateSideAnswers(sideAnswers);
        }

        public IEnumerable<Photo> GetPhotosOfDelivery(int deliveryId)
        {
            return _deliveryRepository.ReadPhotosOfDelivery(deliveryId);
        }

        public Answer GetAnswerWithPhotoQuestion(int deliveryId, int photoQuestionId, int photoId)
        {
            return _deliveryRepository.ReadAnswerWithPhotoQuestionById(deliveryId, photoQuestionId, photoId);
        }

        public IEnumerable<SideAnswer> GetSideAnswersOfAnswer(int answerAnswerId)
        {
            return _deliveryRepository.ReadSideAnswersOfAnswer(answerAnswerId);
        }

        public IEnumerable<Photo> GetAssignedPhotos(int deliveryId, int photoQuestionId)
        {
            var answers = GetAnswersOfPhotoQuestionWithPhotos(deliveryId, photoQuestionId).ToList();

            return answers.Select(t => t.AssignedPhoto).ToList();
        }

        private IEnumerable<Answer> GetAnswersOfPhotoQuestionWithPhotos(int deliveryId, int photoQuestionId)
        {
            return _deliveryRepository.ReadAnswersOfPhotoQuestionWithPhotosById(deliveryId, photoQuestionId);
        }

        public void RemoveAnswer(int answerId)
        {
            _deliveryRepository.RemoveAnswer(answerId);
        }

        public IEnumerable<Photo> GetPhotosWithAnswers(string photoPicture)
        {
            return _deliveryRepository.GetPhotosWithAnswers(photoPicture);
        }

        public void RemovePhotos(IEnumerable<Photo> photosList)
        {
            foreach (var photo in photosList)
            {
                _photoRepository.RemovePhoto(photo);
            }
        }

        public bool SaveDelivery(TaskDelivery delivery)
        {
            if (delivery.Group.AcceptDeliveries)
            {
                // delivery.Student = null;
                delivery.Finished = true;
                _deliveryRepository.UpdateDelivery(delivery);
                RemoveUnusedPhotos(delivery);
                RemoveEmptySideAnswers(delivery);

                return true;
            }

            RemoveDelivery(delivery);
            return false;
        }

        private void RemoveDelivery(TaskDelivery delivery)
        {
            _deliveryRepository.RemoveDelivery(delivery);
        }

        private void RemoveUnusedPhotos(TaskDelivery delivery)
        {
            var usedPhotos = delivery.Answers.Select(a => a.AssignedPhoto).ToList();
            var toDeletePhotos = delivery.SentPhotos.Where(photo => !usedPhotos.Contains(photo)).ToList();

            _photoRepository.RemovePhotos(toDeletePhotos);
        }

        private void RemoveEmptySideAnswers(TaskDelivery delivery)
        {
            var sideAnswerCollections = delivery.Answers.Select(a => a.SideAnswers).ToList();
            var sideAnswers = new List<SideAnswer>();
            foreach (var collection in sideAnswerCollections)
            {
                sideAnswers.AddRange(collection);
            }

            foreach (var sideAnswer in sideAnswers.Where(sideAnswer => string.IsNullOrEmpty(sideAnswer.GivenAnswer)))
            {
                _deliveryRepository.RemoveSideAnswer(sideAnswer);
            }
        }

        public IEnumerable<TaskDelivery> GetDeliveriesForCheckFromStudent(int studentId)
        {
            return _deliveryRepository.ReadDeliveryForCheck(studentId);
        }

        public IEnumerable<TaskDelivery> GetDeliveriesOfGroup(string groupCode)
        {
            return _deliveryRepository.ReadDeliveriesOfGroup(groupCode);
        }

        public Answer GetAnswerById(int answerId)
        {
            return _deliveryRepository.ReadAnswerById(answerId);
        }

        public IEnumerable<Tag> GetTagsOfAnswer(int answerId)
        {
            var answer = _deliveryRepository.ReadAnswerWithDeliveryAndTagsById(answerId);
            return answer.TaskDelivery.Tags;
        }

        public IEnumerable<TaskDelivery> GetSetDeliveriesOfSetTask(SetTask task)
        {
            return _deliveryRepository.ReadSetDeliveriesOfSetTask(task);
        }

        public void UpdateQualityScore(int answerId)
        {
            var answer = _deliveryRepository.GetAnswerWithQualityScoreEvents(answerId);
            answer.QualityScore = answer.QualityScoreEvents.Sum(@event => @event.Score);
            answer.IsQuarantined = answer.QualityScore <= QuarantineTreshold;
            _deliveryRepository.UpdateAnswer(answer);
        }

        public void SetFlaggedAnswers(string picture)
        {
            var answers = _deliveryRepository.GetAnswers(picture);
            foreach (var answer in answers)
            {
                answer.Flagged = true;
            }

            _deliveryRepository.UpdateAnswers(answers);
        }

        public IEnumerable<Answer> GetMarkedAnswers(List<SetUp> adminSetUps)
        {
            List<Answer> markedAnswers = new List<Answer>();

            for (int i=0; i<adminSetUps.Count();i++)
            {
                markedAnswers.AddRange(_deliveryRepository.ReadMarkedAnswersOfSetUp(adminSetUps[i]));
            }

            return markedAnswers;
        }

        public void UnmarkAnswer(int answerId)
        {
            var answer = _deliveryRepository.ReadAnswerById(answerId);
            answer.IsQuarantined = false;
            _deliveryRepository.UpdateAnswer(answer);
        }

        public void UpdateAnswer(Answer answer)
        {
            _deliveryRepository.UpdateAnswer(answer);
        }

        public void UpdateDelivery(TaskDelivery delivery)
        {
            _deliveryRepository.UpdateDelivery(delivery);
        }

        public Answer GetAnswerWithCustomTagsById(int answerId)
        {
            return _deliveryRepository.ReadAnswerWithTagsById(answerId);
        }

        public void RemoveTag(int answerId, string value)
        {
            _deliveryRepository.DeleteTag(answerId, value);
        }


        public void RemoveAnswer(int photoQuestionId, int photoId)
        {
            _deliveryRepository.RemoveAnswer(photoQuestionId, photoId);
        }
    }
}