// using System.Collections.Generic;
// using System.Linq;
// using POC.BL.Domain.delivery;
// using POC.BL.Domain.setup;
// using POC.BL.Domain.task;
// using POC.BL.Domain.user;
// using POC.DAL;
// using POC.DAL.repo;
// using TechTalk.SpecFlow;
// using TechTalk.SpecFlow.Assist;
// using Xunit;
//
// namespace Tests.Steps
// {
//     [Binding]
//     [Scope(Feature = "opdrachten maken")]
//     public class CreateTaskSteps
//     {
//         #region attributes        
//         private List<Teacher> _teachers = new List<Teacher>();
//         private List<PhotoQuestion> _photoQuestions = new List<PhotoQuestion>();
//         private List<SideQuestion> _sideQuestions = new List<SideQuestion>();
//         private List<Answer> _answers = new List<Answer>();
//         private List<SideAnswer> _sideAnswers = new List<SideAnswer>();
//         private List<Photo> _photos = new List<Photo>();
//         
//         private ITaskRepository _taskRepository = InMemoryTaskRepository.GetInstance();
//         private ISetupRepository _setupRepository = InMemorySetupRepository.GetInstance();
//         private IGroupRepository _groupRepository = InMemoryGroupRepository.GetInstance();
//         private IStudentRepository _studentRepository = InMemoryStudentRepository.GetInstance();
//         private IDeliveryRepository _deliveryRepository = InMemoryDeliveryRepository.GetInstance();
//         #endregion
//         
//         [Given(@"students:")]
//         public void GivenStudents(Table givenStudents)
//         {
//             var students = givenStudents.CreateSet<Student>();
//             foreach (var student in students)
//             {
//                 _studentRepository.AddStudent(student);
//             }
//         }
//
//         [Given(@"teachers:")]
//         public void GivenTeachers(Table givenTeachers)
//         {
//             _teachers = givenTeachers.CreateSet<Teacher>().ToList();
//         }
//
//         [Given(@"groups:")]
//         public void GivenGroups(Table givenGroups)
//         {
//             var groups = givenGroups.CreateSet<Group>();
//             foreach (var group in groups)
//             {
//                 _groupRepository.CreateGroup(group);
//             }
//         }
//         
//         [Given(@"tasks:")]
//         public void GivenTasks(Table givenTasks)
//         {
//             var tasks = givenTasks.CreateSet<SetTask>();
//             foreach (var task in tasks)
//             {
//                 _taskRepository.AddSetTask(task);
//             }
//         }
//         
//         [Given(@"photoQuestions:")]
//         public void GivenPhotoQuestions(Table givenPhotoQuestions)
//         {
//             _photoQuestions = givenPhotoQuestions.CreateSet<PhotoQuestion>().ToList();
//         }
//         
//         [Given(@"sideQuestions:")]
//         public void GivenSideQuestions(Table givenSideQuestions)
//         {
//             var rows = givenSideQuestions.CreateSet<List<string>>().ToList();
//             for (var i = 0; i < rows.Count(); i++)
//             {
//                 var columns = rows[i];
//                 if (columns[2] == null)
//                 {
//                     var openQuestion = new OpenSideQuestion()
//                     {
//                         Id = (byte) i,
//                         Question = columns[1],
//                         PhotoQuestion = _photoQuestions.Find(question => question.Id.ToString().Equals(columns[3]))
//                     };
//                     _sideQuestions.Add(openQuestion);
//                 }
//                 else
//                 {
//                     var mcQuestion = new MultipleChoiceSideQuestion()
//                     {
//                         Id = (byte) i,
//                         Question = columns[1],
//                         PhotoQuestion = _photoQuestions.Find(question => question.Id.ToString().Equals(columns[3])),
//                         Options = columns[2].Split(",")
//                     };
//                     _sideQuestions.Add(mcQuestion);
//                 }
//             }
//         }
//
//         [Given(@"taskDeliveries:")]
//         public void GivenTaskDeliveries(Table givenTaskDeliveries)
//         {
//             var deliveries = givenTaskDeliveries.CreateSet<SetTaskDelivery>();
//             foreach (var delivery in deliveries)
//             {
//                 _deliveryRepository.AddDelivery(delivery);
//             }
//         }
//         
//         [Given(@"setups:")]
//         public void GivenSetups(Table givenSetup)
//         {
//             var setups = givenSetup.CreateSet<SetUp>();
//             foreach (var setup in setups)
//             {
//                 _setupRepository.AddSetup(setup);
//             }
//         }
//         
//         [Given(@"answers:")]
//         public void GivenAnswers(Table givenAnswers)
//         {
//             _answers = givenAnswers.CreateSet<Answer>().ToList();
//         }
//         
//         [Given(@"side-answers:")]
//         public void GivenSideAnswers(Table givenSideAnswers)
//         {
//             var rows = givenSideAnswers.CreateSet<List<string>>().ToList();
//             for (var i = 0; i < rows.Count(); i++)
//             {
//                 var columns = rows[i];
//                 var sideQuestion = _sideQuestions[int.Parse(columns[2])-1];
//
//                 switch (sideQuestion)
//                 {
//                     case OpenSideQuestion openSideQuestion:
//                     {
//                         var openAnswer = new OpenAnswer()
//                         {
//                             Id = (byte) i,
//                             GivenAnswer = columns[1],
//                             AnsweredQuestion = openSideQuestion
//                         };
//                         _sideAnswers.Add(openAnswer);
//                         break;
//                     }
//                     case MultipleChoiceSideQuestion mcSideQuestion:
//                     {
//                         var mcAnswer = new MultipleChoiceAnswer()
//                         {
//                             Id = (byte) i,
//                             ChosenValue = columns[1],
//                             AnsweredQuestion = mcSideQuestion
//                         };
//                         _sideAnswers.Add(mcAnswer);
//                         break;
//                     }
//                 }
//             }
//         }
//
//         [Given(@"photos:")]
//         public void GivenPhotos(Table givenPhotos)
//         {
//             _photos = givenPhotos.CreateSet<Photo>().ToList();
//         }
//
//         private OpenAnswer sideAnswer;
//         [When(@"Student answers sideQuestion (.+) with (.+)")]
//         public void WhenStudentAnswersSideQuestionWith(string p2, string p1)
//         {
//             var sideQuestion = (OpenSideQuestion) _sideQuestions.Find(sq => sq.Question == p2);
//             sideAnswer = new OpenAnswer()
//             {
//                 Id = (byte) (_sideAnswers.Count + 1),
//                 GivenAnswer = p1,
//                 AnsweredQuestion = sideQuestion
//             };
//             _sideAnswers.Add(sideAnswer);
//         }
//         [Then(@"SideAnswer is given the value (.+)")]
//         public void ThenSideAnswerIsGivenTheValue(string p1)
//         {
//             Assert.Equal(sideAnswer.GivenAnswer,p1);
//         }
//     }
// }