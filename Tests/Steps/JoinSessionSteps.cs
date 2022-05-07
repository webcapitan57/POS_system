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
    [Scope(Feature = "deelnemen sessie")]
    public class JoinGroupSteps
    {
        #region attributes

        private ICollection<StudentProfileQuestion> _studentProfileQuestions = new List<StudentProfileQuestion>();
        private ICollection<StudentProfileAnswer> _studentProfileAnswers = new List<StudentProfileAnswer>();
        private ITaskRepository _taskRepository = InMemoryTaskRepository.GetInstance();
        private ISetupRepository _setupRepository = InMemorySetupRepository.GetInstance();
        private List<Teacher> _teachers = new List<Teacher>();
        private IGroupRepository _groupRepository = InMemoryGroupRepository.GetInstance();
        private IStudentRepository _studentRepository = InMemoryStudentRepository.GetInstance();
        private IDeliveryRepository _deliveryRepository = InMemoryDeliveryRepository.GetInstance();

        #endregion

        #region background
        [Given(@"StudentProfileQuestions:")]
        public void GivenStudentProfileQuestions(Table givenStudentProfileQuestions)
        {
            _studentProfileQuestions.Clear();
            _studentProfileQuestions = givenStudentProfileQuestions.CreateSet<StudentProfileQuestion>().ToList();
        }

        [Given(@"Setup:")]
        public void GivenSetup(Table givenSetup)
        {
            _setupRepository.ClearSetups();
            var rows = givenSetup.Rows;

            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var setUp = new SetUp()
                {
                    Id = int.Parse(columns.Values.ToList()[0])
                };
                var profileQuestions = columns.Values.ToList()[1].Split(",");

                foreach (var profileQuestion in profileQuestions)
                {
                    var value = int.Parse(profileQuestion);
                    setUp.NeededStudentProfileQuestions ??= new List<StudentProfileQuestion>();
                    setUp.NeededStudentProfileQuestions.Add(_studentProfileQuestions.ToList()
                        .Find(spq => spq.Id == value));
                }
                _setupRepository.AddSetup(setUp);
            }
        }

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
                    SetUp = _setupRepository.ReadSetupById(int.Parse(columns.Values.ToList()[1]))
                };
                _teachers.Add(teacher);
            }
        }

        [Given(@"Tasks:")]
        public void GivenTasks(Table givenTasks)
        {
            _taskRepository.ClearTasks();
            var rows = givenTasks.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var task = new SetTask()
                {
                    Id = int.Parse(columns.Values.ToList()[0]),
                    Title = columns.Values.ToList()[1]
                };
                _taskRepository.AddSetTask(task);
            }
        }

        [Given(@"Groups:")]
        public void GivenGroups(Table givenGroups)
        {
            _groupRepository.ClearGroups();
            _deliveryRepository.ClearTaskDeliveries();
            var rows = givenGroups.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var teacherId = int.Parse(columns.Values.ToList()[2]);
                var group = new Group()
                {
                    GroupCode = columns.Values.ToList()[0],
                    Teacher = _teachers.Find(t=>t.UserId == teacherId)
                };
                
                var tasks = columns.Values.ToList()[1].Split(",");

                foreach (var task in tasks)
                {
                    if (!task.Equals(""))
                    {
                        var value = int.Parse(task); 
                        group.Tasks ??= new List<Task>();
                        group.Tasks.Add(_taskRepository.ReadTaskById(value));
                    }
                }
                _groupRepository.CreateGroup(group);
            }
        }

        [Given(@"StudentProfileAnswers:")]
        public void GivenStudentProfileAnswers(Table givenStudentProfileAnswers)
        {
            _studentProfileAnswers.Clear();
            var rows = givenStudentProfileAnswers.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var value = int.Parse(columns.Values.ToList()[1]);
                var studentProfileAnswer = new StudentProfileAnswer()
                {
                    Id = int.Parse(columns.Values.ToList()[0]),
                    AnsweredQuestion = _studentProfileQuestions.ToList().Find(spq=>spq.Id == value),
                    Value = columns.Values.ToList()[2]
                };
                _studentProfileAnswers.Add(studentProfileAnswer);
            }
        }

        [Given(@"Students:")]
        public void GivenStudents(Table givenStudents)
        {
            _studentRepository.ClearStudents();
            var rows = givenStudents.Rows;
            for (var i = 0; i < rows.Count(); i++)
            {
                var columns = rows[i];
                var groupId = columns.Values.ToList()[2];
                var student = new Student()
                {
                    Id = short.Parse(columns.Values.ToList()[0]),
                    Group = _groupRepository.ReadGroupByCode(groupId)
                };
                
                var profileAnswers = columns.Values.ToList()[1].Split(",");

                foreach (var task in profileAnswers)
                {
                    if (!task.Equals(""))
                    {
                        var value = int.Parse(task); 
                        student.ProfileAnswers ??= new List<StudentProfileAnswer>();
                        student.ProfileAnswers.Add(_studentProfileAnswers.ToList().Find(spa=>spa.Id == value));
                    }
                }
                _studentRepository.AddStudent(student);
            }
        }
        #endregion

        [When(@"A user inputs '(.*)' in the group code field")]
        public void WhenAUserInputs1A1B1CInTheGroupCodeFieldCheckIfGroup1A1B1CExists(string s1)
        {
            
            try
            {
                Group group = _groupRepository.ReadGroupByCode(s1);
                {
                    Student student = new Student()
                    {
                        Id = (short)(_studentRepository.NumberOfStudents()+1),
                        Group = group
                    };
                           
                    var questions = group.Teacher.SetUp.NeededStudentProfileQuestions.ToList();
                    foreach (var question in questions)
                    {
                        StudentProfileAnswer answer = new StudentProfileAnswer()
                        {
                            Id = _studentProfileAnswers.Count + 1,
                            AnsweredQuestion = question
                        };
                        student.ProfileAnswers ??= new List<StudentProfileAnswer>();
                        student.ProfileAnswers.Add(answer);
                        _studentProfileAnswers.Add(answer);
                    }
                    _studentRepository.AddStudent(student); 
                }
            }
            catch (Exception e)
            {
                
            }
        }

        [Then(@"student '(.*)' is created")]
        public void ThenStudent2IsCreatedCreateStudent3CheckIfExists(int p1)
        {
            Assert.NotNull(_studentRepository.ReadStudentById(p1));
        }

        [Then(@"student '(.*)' is linked to group '(.*)'")]
        public void ThenStudent2IsLinkedToGroup1A1B1C(int p1, string s1)
        {
            Assert.True(_studentRepository.ReadStudentById(p1).Group.GroupCode.Equals(s1));
        }

        [Then(@"StudentProfileAnswer '(.*)' is created and linked to Student '(.*)' and studentProfileQuestion '(.*)'")]
        public void ThenStudentProfileAnswer5IsCreatedAndLinkedToStudent2(int p1,int p2,int p3)
        {
            var answers = _studentRepository.ReadStudentById(p2).ProfileAnswers.ToList();
            Assert.NotNull(answers.Find(a=>a.Id==p1));
            foreach (var answer in answers)
            {
                Assert.NotNull(answer.AnsweredQuestion);
            }
        }
        
        [Then(@"There is no student '(.*)'")]
        public void ThenThereIsNoStudent3(int p1)
        {
            Assert.Null(_studentRepository.ReadStudentById(p1));
        }

        [When(@"Student '(.*)' gives studentProfileAnswer '(.*)' value '(.*)' to StudentProfileQuestion '(.*)'")]
        public void WhenStudent2GivesStudentProfileAnswer5ValueFemaleAsAnAnswerToStudentProfileQuestion1(int p1, int p2,string s1, int p3)
        {
            Student student = _studentRepository.ReadStudentById(p1);
            StudentProfileAnswer answer = student.ProfileAnswers.ToList().Find(a => a.Id == p2);
            answer.Value = s1;
            if (student.ProfileAnswers.ToList().FindIndex(a => a.Id == p2)
                == student.ProfileAnswers.ToList().Count - 1)
            {
                foreach (var task in student.Group.Tasks)
                {
                    SetTaskDelivery delivery = new SetTaskDelivery()
                    {
                        Id = _deliveryRepository.NumberOfDeliveries()+1,
                        SetTask = (SetTask)task,
                        Group = student.Group
                    }; 
                    _deliveryRepository.AddDelivery(delivery);
                    
                }
            }
            
        }
        [When(@"Student '(.*)' gives studentProfileAnswer '(.*)' no value to StudentProfileQuestion '(.*)'")]
        public void WhenStudent3GivesStudentProfileAnswer8NoValueToStudentProfileQuestion2(int p1, int p2, int p3)
        {
            Student student = _studentRepository.ReadStudentById(p1);
            StudentProfileAnswer answer = student.ProfileAnswers.ToList().Find(a => a.Id == p2);
            answer.Value = null;
            if (student.ProfileAnswers.ToList().FindIndex(a => a.Id == p2)
                == student.ProfileAnswers.ToList().Count - 1)
            {
                foreach (var task in student.Group.Tasks)
                {
                    SetTaskDelivery delivery = new SetTaskDelivery()
                    {
                        Id = _deliveryRepository.NumberOfDeliveries()+1,
                        SetTask = (SetTask)task,
                        Group = student.Group
                    }; 
                    _deliveryRepository.AddDelivery(delivery);
                    
                }
            }
        }
        [Then(@"Value of studentProfileAnswer '(.*)' is '(.*)'")]
        public void ThenValueOfStudentProfileAnswer5IsFemale(int p1, string s1)
        {
            Assert.True(_studentProfileAnswers.ToList().Find(spa=>spa.Id == p1).Value.Equals(s1));
        }
        [Then(@"Value of studentProfileAnswer '(.*)' is null")]
        public void ThenValueOfStudentProfileAnswer8IsNull(int p1)
        {
            Assert.Null(_studentProfileAnswers.ToList().Find(spa=>spa.Id == p1).Value);
        }
        [Then(@"Delivery '(.*)' is created")]
        public void ThenDelivery3IsCreated(int p1)
        {
            Assert.NotNull(_deliveryRepository.ReadDeliveryWithStudentGroupAndTasksById(p1));
        }

        [Then(@"Delivery '(.*)' is linked to group '(.*)'")]
        public void ThenDelivery3IsLinkedToGroup1A1B1C(int p1, string s1)
        {
            TaskDelivery delivery = _deliveryRepository.ReadDeliveryWithStudentGroupAndTasksById(p1);
            Assert.True(delivery.Group.GroupCode.Equals(s1));
        }

        
        [When(@"student '(.*)' leaves all question fields blank and presses next")]
        public void WhenStudent2LeavesAllQuestionFieldsBlankAndPressesNext(int p1)
        {
            Student student = _studentRepository.ReadStudentById(p1);
            bool answeredRequered = false;
            foreach (var answer in student.ProfileAnswers)
            {
                if (!answeredRequered)
                {
                    answeredRequered = answer.AnsweredQuestion.IsRequired;
                }
            }
            if(answeredRequered)
            {
                foreach (var task in student.Group.Tasks)
                {
                    SetTaskDelivery delivery = new SetTaskDelivery()
                    {
                        Id = _deliveryRepository.NumberOfDeliveries()+1,
                        SetTask = (SetTask)task,
                        Group = student.Group
                    }; 
                    _deliveryRepository.AddDelivery(delivery);
                    
                }
            }
        }

        [Then(@"no deliveries are created")]
        public void ThenNoDeliveriesAreCreated()
        {
            Assert.True(_deliveryRepository.NumberOfDeliveries() == 0);
        }
    }
}