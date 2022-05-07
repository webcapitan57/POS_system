using POC.DAL.EF.SC.DAL.EF;

namespace POC.DAL.EF
{
    internal static class Initializer
    {
        private static bool _hasRunDuringAppExecution = false;

        public static void Initialize(PhotoAppDbContext context, bool dropCreateDatabase = true)
        {
            if (!_hasRunDuringAppExecution)
            {
                // Delete database if requesed
                if (dropCreateDatabase)
                    context.Database.EnsureDeleted();

                // Create database and seed dummy-data if needed 
                if (context.Database.EnsureCreated()) // 'false' if database already exists
                    // Seed initial (dummy-)data into newly created database
                    Seed(context);
                _hasRunDuringAppExecution = true;
            }
        }

        public static void Seed(PhotoAppDbContext context)
        {
           /* #region Setup

            SetUp setUp1 = new SetUp()
            {
                
                Name = "Karel De Grote",
                Archived = false,
                GeneralText = "Algemene Tekst van Setup 1",
                SetTasks = new List<SetTask>(),
                Teachers = new List<Teacher>(),
                NeededProfileQuestions = new List<ProfileQuestion>(),
                Admins = new List<SetUpAdmin>(),
                PrimColor = "#ff0000",
                SecColor = "#011435",
            };
            SetUp setUp2 = new SetUp()
            {
                Name = "VRT Edubox",
                CreationDate = DateTime.Now,
                Archived = false,
               GeneralText = "Algemene Tekst van Setup 2",
                SetTasks = new List<SetTask>(),
                Teachers = new List<Teacher>(),
                NeededProfileQuestions = new List<ProfileQuestion>(),
                PrimColor = "#57BE62",
                SecColor = "#011435",
            };

            Admin admin1 = new Admin()
            {
                Username = "Test",
                SetUps = new List<SetUpAdmin>()
            };

            SetUpAdmin sa1 = new SetUpAdmin()
            {
                SetUp = setUp1,
                Admin = admin1
            };

            #endregion

            #region ProfileQuestions

            var studentProfileQuestion1 = new StudentProfileQuestion()
            {
                Description = "leeftijd",
                Question = "Wat is je leeftijd?",
                IsRequired = true,
                SetUp = setUp1
            };
            setUp1.NeededProfileQuestions.Add(studentProfileQuestion1);

            var studentProfileQuestion2 = new StudentMCProfileQuestion()
            {
                Description = "Geslacht",
                IsRequired = true,
                Question = "Wat is je geslacht?",
                SetUp = setUp1,
                StudentProfileOptions = new List<StudentProfileOption>()
            };
            studentProfileQuestion2.StudentProfileOptions.Add(new StudentProfileOption()
            {
                StudentMcProfileQuestion = studentProfileQuestion2,
                Value = "Man"
            });
            studentProfileQuestion2.StudentProfileOptions.Add(new StudentProfileOption()
            {
                StudentMcProfileQuestion = studentProfileQuestion2,
                Value = "Vrouw"
            });
            studentProfileQuestion2.StudentProfileOptions.Add(new StudentProfileOption()
            {
                StudentMcProfileQuestion = studentProfileQuestion2,
                Value = "Andere"
            });
            setUp1.NeededProfileQuestions.Add(studentProfileQuestion2);

            
            var groupProfileQuestion1 = new GroupProfileQuestion()
            {
                Description = "leeftijd",
                Question = "Wat is de gemiddelde leeftijd van je etters?",
                IsRequired = true,
                SetUp = setUp1
            };
            setUp1.NeededProfileQuestions.Add(groupProfileQuestion1);

            var groupProfileQuestion2 = new GroupMCProfileQuestion()
            {
                Description = "Studierichting",
                IsRequired = true,
                Question = "Waar heb je geleden?",
                SetUp = setUp1,
                GroupProfileOptions = new List<GroupProfileOption>()
            };
            groupProfileQuestion2.GroupProfileOptions.Add(new GroupProfileOption()
            {
                GroupMcProfileQuestion = groupProfileQuestion2,
                Value = "BSO"
            });
            groupProfileQuestion2.GroupProfileOptions.Add(new GroupProfileOption()
            {
                GroupMcProfileQuestion = groupProfileQuestion2,
                Value = "KSO"
            });
            groupProfileQuestion2.GroupProfileOptions.Add(new GroupProfileOption()
            {
                GroupMcProfileQuestion = groupProfileQuestion2,
                Value = "ASO"
            });
            groupProfileQuestion2.GroupProfileOptions.Add(new GroupProfileOption()
            {
                GroupMcProfileQuestion = groupProfileQuestion2,
                Value = "TSO"
            });
            setUp1.NeededProfileQuestions.Add(studentProfileQuestion2);
            #endregion

            #region Task
            
            var setTask1 = new SetTask()
            {
                Title = "Muziek",
                SetUp = setUp1,
                Info = "Een taak over muziek",
                Questions = new List<PhotoQuestion>(),
                Groups = new List<GroupTask>()
            };
            
            setUp1.SetTasks.Add(setTask1);

            var setTask2 = new SetTask()
            {
                Title = "SetTaak 2",
                Info = "Dit is settaak 2",
                SetUp = setUp1,
                Questions = new List<PhotoQuestion>(),
                Groups = new List<GroupTask>()
            };
            setUp1.SetTasks.Add(setTask2);

            var setTask3 = new SetTask()
            {
                Title = "SetTaak 3",
                Info = "Dit is settaak 3",
                SetUp = setUp1,
                Questions = new List<PhotoQuestion>(),
                Groups = new List<GroupTask>()
            };
            setUp1.SetTasks.Add(setTask3);

            #endregion

            #region PhotoQuestion

            var photoQuestion1 = new PhotoQuestion()
            {
                Task = setTask1,
                Question = "Welke artiest vind je leuk?",
                SideQuestions = new List<SideQuestion>(),
                Tips = "Upload een foto van een artiest die je leuk vind"
            };
            setTask1.Questions.Add(photoQuestion1);

            var photoQuestion2 = new PhotoQuestion()
            {
                Task = setTask1,
                Question = "Welke artiest vind je niet leuk?",
                SideQuestions = new List<SideQuestion>()
            };
            setTask1.Questions.Add(photoQuestion2);

            var photoQuestion3 = new PhotoQuestion()
            {
                Task = setTask2,
                Question = "TestVraag",
                SideQuestions = new List<SideQuestion>(),
                Tips = "Upload hier gwn wat foto's"
            };
            setTask2.Questions.Add(photoQuestion3);

            #endregion

            #region SideQuestions

            var sideQuestion1 = new SideQuestion()
            {
                
                PhotoQuestion = photoQuestion1,
                Question = "Wat is de naam van deze artiest?",
            };
            photoQuestion1.SideQuestions.Add(sideQuestion1);

            var sideQuestion2 = new SideQuestion()
            {
                
                PhotoQuestion = photoQuestion2,
                Question = "Waarom vind je deze artiest niet leuk?",
                SideQuestionOptions = new List<SideQuestionOption>()
            };
            photoQuestion2.SideQuestions.Add(sideQuestion2);
            
            var sideQuestion3 = new SideQuestion()
            {
                
                PhotoQuestion = photoQuestion1,
                Question = "Hoeveel uur per week luister je naar deze artiest?",
            };
            photoQuestion1.SideQuestions.Add(sideQuestion3);

            var sideQuestion4 = new SideQuestion()
            {
                PhotoQuestion = photoQuestion3,
                Question = "een Bijvraag?"
            };
            photoQuestion3.SideQuestions.Add(sideQuestion4);

            #endregion

            #region SideQuestionOptions

            var option0 = new SideQuestionOption()
            {
                Value = "Niet mijn genre.",
                MultipleChoiceSideQuestion = sideQuestion2
            };
            sideQuestion2.SideQuestionOptions.Add(option0);

            var option1 = new SideQuestionOption()
            {
                Value = "Ik vind deze artiest niet goed.",
                MultipleChoiceSideQuestion = sideQuestion2
            };
            sideQuestion2.SideQuestionOptions.Add(option1);

            #endregion

            #region Teacher

            Teacher teacher1 = new Teacher()
            {
                Username = "Johannes The Rich",
                TeacherTasks = new List<TeacherTask>(),
                Groups = new List<Group>(),
                IsGuest = false,
                QualityScoreEvents = new List<QualityScoreEvent>(),
                SetUp = setUp1


            };
            setUp1.Teachers.Add(teacher1);
            
            Teacher teacher2 = new Teacher()
            {
                Username = "Johannes The Poor",
                TeacherTasks = new List<TeacherTask>(),
                Groups = new List<Group>(),
                IsGuest = false,
                QualityScoreEvents = new List<QualityScoreEvent>(),
                SetUp = setUp1
                
    
            };
            setUp1.Teachers.Add(teacher2);

            #endregion
            
            #region Group

            Group group1 = new Group()
            {
                GroupCode = "1",
                Name = "Make VRT Great Again",
                Tasks = new List<GroupTask>(),
                Teacher = teacher1,
                AcceptDeliveries = true
            };
            
            Group group2 = new Group()
            {
                GroupCode = "2",
                Name = "Make VRT Bad Again",
                Tasks = new List<GroupTask>(),
                Teacher = teacher1,
                AcceptDeliveries = false
                
            };
            teacher1.Groups.Add(group1);
            teacher1.Groups.Add(group2);
            #endregion
            
            #region GroupTask

            GroupTask groupTask1 = new GroupTask()
            {
                Task = setTask1,
                Group = group1
            };
            GroupTask groupTask2 = new GroupTask()
            {
                Task = setTask2,
                Group = group1
            };
           group1.Tasks.Add(groupTask2);
           setTask2.Groups.Add(groupTask2);
            
            var groupTask3 = new GroupTask()
            {
                Task = setTask1,
                Group = group2
            };
           group2.Tasks.Add(groupTask3);
           setTask1.Groups.Add(groupTask3);
            
            #region GroupInfo

            GroupInfo groupInfo1 = new GroupInfo()
            {
                Group = group1,
                SetUp = setUp1,
                TeacherName = group1.Teacher.Username
            };
            
            GroupInfo groupInfo2 = new GroupInfo()
            {
                TeacherName = group2.Teacher.Username,
                Group = group2,
                SetUp = setUp1
            };
            group1.Info = groupInfo1;
            group2.Info = groupInfo2;

            #endregion

            
            

            #endregion

            
            admin1.SetUps = new List<SetUpAdmin> {sa1};
            setUp1.Admins = new List<SetUpAdmin> {sa1};
            
            context.SetUps.Add(setUp1);
            context.Admins.Add(admin1);
            context.SetUpAdmins.Add(sa1);
            context.SetUps.Add(setUp2);
            context.GroupTasks.Add(groupTask1);
            context.GroupTasks.Add(groupTask2);
            context.GroupInfos.Add(groupInfo1);
            context.GroupInfos.Add(groupInfo2);


            context.SaveChanges();

            foreach (var entry in context.ChangeTracker.Entries().ToList())
            {
                entry.State = EntityState.Detached;
            }*/
        }
    }
}