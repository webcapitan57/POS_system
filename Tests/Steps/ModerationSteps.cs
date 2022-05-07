using System;
using System.Collections.Generic;
using System.Linq;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.DAL;
using POC.DAL.repo;
using POC.DAL.repo.InMemory;
using POC.DAL.repo.InterFaces;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Tests.Steps
{
    [Binding]
    [Scope(Feature = "leerkracht foto's modereren")]
    public class ModerationSteps
    {
        #region attributes

        private List<Teacher> _teachers = new List<Teacher>();
        private List<PhotoQuestion> _photoQuestions = new List<PhotoQuestion>();
        private List<SideQuestion> _sideQuestions = new List<SideQuestion>();
        private List<Answer> _answers = new List<Answer>();
        private List<SideAnswer> _sideAnswers = new List<SideAnswer>();
        private List<Photo> _photos = new List<Photo>();

        private ITaskRepository _taskRepository = InMemoryTaskRepository.GetInstance();
        private ISetupRepository _setupRepository = InMemorySetupRepository.GetInstance();
        private IGroupRepository _groupRepository = InMemoryGroupRepository.GetInstance();
        private IStudentRepository _studentRepository = InMemoryStudentRepository.GetInstance();
        private IDeliveryRepository _deliveryRepository = InMemoryDeliveryRepository.GetInstance();

        #endregion

        [Given(@"Teacher:")]
        public void GivenTeacher(Table givenTeacher)
        {
            _teachers.Clear();
            var rows = givenTeacher.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var teacher = new Teacher()
                {
                    UserId = int.Parse(columns.Values.ToList()[0]),
                    Username = columns.Values.ToList()[1],
                    SetUp = _setupRepository.ReadSetupById(int.Parse(columns.Values.ToList()[2]))
                };
                _teachers.Add(teacher);
            }
        }

        [Given(@"SideQuestions:")]
        public void GivenSideQuestions(Table givenSideQuestions)
        {
            var rows = givenSideQuestions.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                if (columns.Values.ToList()[2] == "")
                {
                    var openQuestion = new OpenSideQuestion()
                    {
                        Id = byte.Parse(columns.Values.ToList()[0]),
                        Question = columns.Values.ToList()[1]
                    };
                    _sideQuestions.Add(openQuestion);
                }
                else
                {
                    var mcQuestion = new MultipleChoiceSideQuestion()
                    {
                        Id = byte.Parse(columns.Values.ToList()[0]),
                        Question = columns.Values.ToList()[1],
                        Options = columns.Values.ToList()[2].Split(",")
                    };
                    _sideQuestions.Add(mcQuestion);
                }
            }
        }

        [Given(@"PhotoQuestions:")]
        public void GivenPhotoQuestions(Table givenPhotoQuestions)
        {
            var rows = givenPhotoQuestions.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var sideQuestions = new List<SideQuestion>();
                var sQIds = columns.Values.ToList()[2].Split(",");

                foreach (var id in sQIds)
                {
                    if (id == "") break;
                    var value = int.Parse(id);
                    sideQuestions.Add(_sideQuestions
                        .Find(sq => sq.Id == (byte) value));
                }

                var photoQuestion = new PhotoQuestion()
                {
                    Id = byte.Parse(columns.Values.ToList()[0]),
                    Question = columns.Values.ToList()[1],
                    SideQuestions = sideQuestions
                };
                _photoQuestions.Add(photoQuestion);
            }
        }

        [Given(@"SetTasks:")]
        public void GivenSetTasks(Table givenSetTasks)
        {
            _taskRepository.ClearTasks();
            var rows = givenSetTasks.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var questions = new List<PhotoQuestion>();
                var qIds = columns.Values.ToList()[2].Split(",");
                foreach (var id in qIds)
                {
                    var value = byte.Parse(id);
                    questions.Add(_photoQuestions.Find(pq => pq.Id == value));
                }

                var task = new SetTask()
                {
                    Id = byte.Parse(columns.Values.ToList()[0]),
                    Title = columns.Values.ToList()[1],
                    Questions = questions
                };
                _taskRepository.AddSetTask(task);
            }
        }

        [Given(@"Groups:")]
        public void GivenGroups(Table givenGroups)
        {
            _groupRepository.ClearGroups();
            var rows = givenGroups.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var tasks = new List<Task>();
                var taskIds = columns.Values.ToList()[2].Split(",");
                foreach (var id in taskIds)
                {
                    var value = int.Parse(id);
                    tasks.Add((SetTask) _taskRepository.ReadTaskById(value));
                }
                var teacherId = int.Parse(columns.Values.ToList()[1]);
                var group = new Group()
                {
                    GroupCode = columns.Values.ToList()[0],
                    Teacher = _teachers.Find(t=>t.UserId == teacherId),
                    Tasks = tasks
                };

                _groupRepository.CreateGroup(group);
            }
        }

        [Given(@"Students:")]
        public void GivenStudents(Table givenStudents)
        {
            _studentRepository.ClearStudents();
            var rows = givenStudents.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var student = new Student()
                {
                    Id = short.Parse(columns.Values.ToList()[0]),
                    Group = _groupRepository.ReadGroupByCode(columns.Values.ToList()[1])
                };

                _studentRepository.AddStudent(student);
            }
        }

        [Given(@"Photos:")]
        public void GivenPhotos(Table givenPhotos)
        {
            _photos.Clear();
            _photos = givenPhotos.CreateSet<Photo>().ToList();
        }

        [Given(@"SideAnswers:")]
        public void GivenSideAnswers(Table givenSideAnswers)
        {
            _sideAnswers.Clear();
            var rows = givenSideAnswers.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var sideQuestion = _sideQuestions.Find(q => q.Id == int.Parse(columns.Values.ToList()[2]));

                switch (sideQuestion)
                {
                    case OpenSideQuestion openSideQuestion:
                    {
                        var openAnswer = new OpenAnswer()
                        {
                            Id = byte.Parse(columns.Values.ToList()[0]),
                            GivenAnswer = columns.Values.ToList()[1],
                            AnsweredQuestion = openSideQuestion,
                        };
                        _sideAnswers.Add(openAnswer);
                        break;
                    }
                    case MultipleChoiceSideQuestion mcSideQuestion:
                    {
                        var mcAnswer = new MultipleChoiceAnswer()
                        {
                            Id = byte.Parse(columns.Values.ToList()[0]),
                            ChosenValue = columns.Values.ToList()[1],
                            AnsweredQuestion = mcSideQuestion,
                        };
                        _sideAnswers.Add(mcAnswer);
                        break;
                    }
                }
            }
        }

        [Given(@"Answers:")]
        public void GivenAnswers(Table givenAnswers)
        {
            _answers.Clear();
            var rows = givenAnswers.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var sideAnswers = new List<SideAnswer>();
                var saId = columns.Values.ToList()[2].Split(",");
                foreach (var id in saId)
                {
                    if (id == "") break;
                    var value = byte.Parse(id);
                    sideAnswers.Add(_sideAnswers.ToList().Find(sa => sa.Id == value));
                }

                Photo photo = null;
                if (columns.Values.ToList()[3] != "")
                {
                    photo = _photos.Find(p => p.Id == byte.Parse(columns.Values.ToList()[3]));
                }

                var answer = new Answer()
                {
                    Id = byte.Parse(columns.Values.ToList()[0]),
                    AnsweredQuestion = _photoQuestions.Find(pq => pq.Id == byte.Parse(columns.Values.ToList()[1])),
                    SideAnswers = sideAnswers,
                    AssignedPhoto = photo
                };

                _answers.Add(answer);
            }
        }

        [Given(@"SetTaskDeliveries:")]
        public void GivenSetTaskDeliveries(Table givenSetTaskDeliveries)
        {
           _deliveryRepository.ClearTaskDeliveries();
            var rows = givenSetTaskDeliveries.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];
                var photos = new List<Photo>();
                var photoIds = columns.Values.ToList()[1].Split(",");

                foreach (var id in photoIds)
                {
                    if (id == "") break;
                    var value = byte.Parse(id);
                    photos.Add(_photos.Find(p => p.Id == value));
                }

                var answers = new List<Answer>();
                var answerIds = columns.Values.ToList()[2].Split(",");

                foreach (var id in answerIds)
                {
                    if (id == "") break;
                    var value = byte.Parse(id);
                    answers.Add(_answers.Find(a => a.Id == value));
                }

                var delivery = new SetTaskDelivery()
                {
                    Id = int.Parse(columns.Values.ToList()[0]),
                    SentPhotos = photos,
                    Answers = answers,
                    SetTask = (SetTask) _taskRepository.ReadTaskById(int.Parse(columns.Values.ToList()[3])),
                    Group = _groupRepository.ReadGroupByCode(columns.Values.ToList()[4])
                };
                _deliveryRepository.AddDelivery((SetTaskDelivery)delivery);
            }
        }

        [Given(@"Setup:")]
        public void GivenSetup(Table givenSetup)
        {
            _setupRepository.ClearSetups();
            var rows = givenSetup.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                var columns = rows[i];

                var tasks = new List<SetTask>();
                var taskId = columns.Values.ToList()[1];
                var value = int.Parse(taskId);
                tasks.Add((SetTask) _taskRepository.ReadTaskById(value));
                
                var setup = new SetUp()
                {
                    Id = int.Parse(columns.Values.ToList()[0]),
                    SetTasks = tasks
                };
                
                _setupRepository.AddSetup(setup);
            }
        }

        [When(@"group '(.*)' deliveres")]
        public void WhenDelivery2IsSubmitted(string s1)
        {
            DeliveryService deliveryService = DeliveryService.GetInstance();
            deliveryService.ScanIncomingDeliveries(_groupRepository.ReadGroupByCode(s1));
        }


        [Then(@"Value flagged in answer '(.*)' changes to true")]
        public void ThenValueFlaggedInAnswer13ChangesToTrue(int p1)
        {
            Assert.True(_answers.ToList().Find(answer => answer.Id == p1).HasFace);
        }

        [Then(@"Value flagged in answer '(.*)' changes to false")]
        public void ThenValueFlaggedInAnswer5ChangesToFalse(int p1)
        {
            Assert.True(!_answers.ToList().Find(answer => answer.Id == p1).HasFace);
        }
    }
}