using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;
using UI_MVC.Gcloud;

namespace UI_MVC.Services
{
    public class IdentityInitializer
    {
        public static void SeedData(UserManager<UserAccount> userManager, RoleManager<IdentityRole> roleManager,
            GcloudStorage storage, IWebHostEnvironment webHostEnvironment)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, storage, webHostEnvironment);
        }

        private static void SeedUsers(UserManager<UserAccount> userManager, GcloudStorage storage,
            IWebHostEnvironment webHostEnvironment)
        {
            if (userManager.FindByEmailAsync("Daniel.Deboeverie@gmail.com").Result == null)
            {
                #region Setup

                var setUp1 = new SetUp()
                {
                    Name = "Karel de Grote",
                    CreationDate = DateTime.Now,
                    Archived = false,
                    GeneralText = "Platform voor Karel de Grote hogeschool",
                    SetTasks = new List<SetTask>(),
                    Teachers = new List<Teacher>(),
                    NeededProfileQuestions = new List<ProfileQuestion>(),
                    Admins = new List<SetUpAdmin>(),
                    PrimColor = "#57BE62",
                    SecColor = "#011435",
                    Usage = new List<GroupInfo>(),
                    loginIndentifier = "EY2URFALJ4PAIJK"
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
                    Usage = new List<GroupInfo>(),
                    loginIndentifier = "RG2XI6DEU97UWGE"
                };

                #region SetUpLogos

                var photo11 = new Photo()
                {
                    SetUp = setUp1,
                    Picture = "kdg.png"
                };
                setUp1.Logo = photo11;

                Photo photo12 = new Photo()
                {
                    SetUp = setUp2,
                    Picture = "defaultLogo.jpg"
                };
                setUp2.Logo = photo12;

                #endregion

                var adminPwd = "Vrt123";
                UserAccount adminUserAccount = new UserAccount()
                {
                    UserName = "MakeVRTGreatAgain",
                    Email = "Daniel.Deboeverie@gmail.com",
                    EmailConfirmed = true
                };


                Admin admin1 = new Admin()
                {
                    UserAccount = adminUserAccount,
                    SetUps = new List<SetUpAdmin>()
                };

                adminUserAccount.User = admin1;
                SetUpAdmin sa1 = new SetUpAdmin()
                {
                    SetUp = setUp1,
                    Admin = admin1
                };
                SetUpAdmin sa2 = new SetUpAdmin()
                {
                    SetUp = setUp2,
                    Admin = admin1
                };
                admin1.SetUps = new List<SetUpAdmin> {sa1, sa2};
                setUp1.Admins = new List<SetUpAdmin> {sa1};
                setUp2.Admins = new List<SetUpAdmin> {sa2};
                var result = userManager.CreateAsync(adminUserAccount, adminPwd).Result;
                UserAccount adminUserAccount2 = new UserAccount()
                {
                    UserName = "Steve.Rogers@kdg.be",
                    Email = "Steve.Rogers@kdg.be",
                    EmailConfirmed = true
                };
                Admin admin2 = new Admin()
                {
                    UserAccount = adminUserAccount2,
                    SetUps = new List<SetUpAdmin>()
                };

                adminUserAccount2.User = admin2;
                SetUpAdmin sa3 = new SetUpAdmin()
                {
                    SetUp = setUp1,
                    Admin = admin2
                };
                SetUpAdmin sa4 = new SetUpAdmin()
                {
                    SetUp = setUp2,
                    Admin = admin2
                };
                admin2.SetUps = new List<SetUpAdmin> {sa3, sa4};
                setUp1.Admins.Add(sa3);
                setUp2.Admins.Add(sa4);

                result = userManager.CreateAsync(adminUserAccount2, adminPwd).Result;

                UserAccount adminUserAccount3 = new UserAccount()
                {
                    UserName = "Tony.Stark@kdg.be",
                    Email = "Tony.Stark@kdg.be",
                    EmailConfirmed = true
                };
                Admin admin3 = new Admin()
                {
                    UserAccount = adminUserAccount3,
                    SetUps = new List<SetUpAdmin>()
                };

                adminUserAccount3.User = admin3;
                SetUpAdmin sa5 = new SetUpAdmin()
                {
                    SetUp = setUp1,
                    Admin = admin3
                };
                admin3.SetUps = new List<SetUpAdmin> {sa5};
                setUp1.Admins.Add(sa5);
                result = userManager.CreateAsync(adminUserAccount3, adminPwd).Result;

                UserAccount adminUserAccount4 = new UserAccount()
                {
                    UserName = "Oswald.Cornelius@kdg.be",
                    Email = "Oswald.Cornelius@kdg.be",
                    EmailConfirmed = true
                };
                Admin admin4 = new Admin()
                {
                    UserAccount = adminUserAccount4,
                    SetUps = new List<SetUpAdmin>()
                };
                adminUserAccount4.User = admin4;
                SetUpAdmin sa6 = new SetUpAdmin()
                {
                    SetUp = setUp2,
                    Admin = admin4
                };
                admin4.SetUps = new List<SetUpAdmin> {sa6};
                setUp2.Admins.Add(sa6);
                result = userManager.CreateAsync(adminUserAccount4, adminPwd).Result;

                #endregion

                #region ProfileQuestions

                var studentProfileQuestion1 = new StudentProfileQuestion()
                {
                    Description = "Leeftijd",
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
                    Description = "Jaar",
                    Question = "In welk jaar zit deze groep?",
                    IsRequired = true,
                    SetUp = setUp1
                };
                setUp1.NeededProfileQuestions.Add(groupProfileQuestion1);

                var groupProfileQuestion2 = new GroupMCProfileQuestion()
                {
                    Description = "Onderwijsvorm",
                    IsRequired = true,
                    Question = "Voor welke onderwijsvorm is deze groep bedoeld?",
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

                SetTask setTask1 = new SetTask()
                {
                    Title = "Antwerpen",
                    SetUp = setUp1,
                    Questions = new List<PhotoQuestion>(),
                    Groups = new List<GroupTask>(),
                    SetTaskDeliveries = new List<SetTaskDelivery>()
                };

                setUp1.SetTasks.Add(setTask1);

                SetTask setTask2 = new SetTask()
                {
                    Title = "Hobbies",
                    SetUp = setUp1,
                    Questions = new List<PhotoQuestion>(),
                    Groups = new List<GroupTask>(),
                    SetTaskDeliveries = new List<SetTaskDelivery>()
                };
                setUp1.SetTasks.Add(setTask2);

                TeacherTask teacherTask = new TeacherTask()
                {
                    Title = "Dieren",
                    Groups = new List<GroupTask>(),
                    TeacherTaskDeliveries = new List<TeacherTaskDelivery>()
                };

                #endregion

                #region PhotoQuestion

                var photoQuestion1 = new PhotoQuestion()
                {
                    Task = setTask1,
                    Question = "Welke plek bezoek je het vaakst?",
                    SideQuestions = new List<SideQuestion>(),
                    Tips = "Zoek of neem een foto van een plek die je vaak bezoekt in Antwerpen.",
                    Answers = new List<Answer>(),
                    Locations = new List<Location>()
                };

                var photoQuestion2 = new PhotoQuestion()
                {
                    Task = setTask1,
                    Question = "Welke plek vind je het mooist?",
                    SideQuestions = new List<SideQuestion>(),
                    Tips = "Zoek of neem een foto van een plek die je mooi vind in Antwerpen",
                    Answers = new List<Answer>(),
                    Locations = new List<Location>()
                };
                var photoQuestion3 = new PhotoQuestion()
                {
                    Task = setTask2,
                    Question = "Welke hobbies beoefen je in je vrije tijd?",
                    SideQuestions = new List<SideQuestion>(),
                    Tips = "Zoek of neem een foto van de plaats of van de uitrusting die je nodig hebt.",
                    Answers = new List<Answer>()
                };

                setTask1.Questions.Add(photoQuestion1);
                setTask1.Questions.Add(photoQuestion2);
                setTask2.Questions.Add(photoQuestion3);

                #endregion

                #region Locations

                var location1 = new Location()
                {
                    Latitude = 51.2186,
                    Longitude = 4.4014,
                    Radius = 2,
                    PhotoQuestion = photoQuestion1
                };
                photoQuestion1.Locations.Add(location1);

                var location2 = new Location()
                {
                    Latitude = 51.2000,
                    Longitude = 4.4090,
                    Radius = 2,
                    PhotoQuestion = photoQuestion1
                };
                photoQuestion1.Locations.Add(location2);


                var location3 = new Location()
                {
                    Latitude = 51.2186,
                    Longitude = 4.4014,
                    Radius = 2,
                    PhotoQuestion = photoQuestion2
                };
                photoQuestion2.Locations.Add(location3);

                var location4 = new Location()
                {
                    Latitude = 51.2000,
                    Longitude = 4.4090,
                    Radius = 2,
                    PhotoQuestion = photoQuestion2
                };
                photoQuestion2.Locations.Add(location4);

                #endregion

                #region SideQuestions

                var sideQuestion1 = new SideQuestion()
                {
                    PhotoQuestion = photoQuestion1,
                    Question = "Hoeveel keer per maand kom je hier gemiddeld?",
                    SideAnswers = new List<SideAnswer>()
                };
                photoQuestion1.SideQuestions.Add(sideQuestion1);

                var sideQuestion2 = new SideQuestion()
                {
                    PhotoQuestion = photoQuestion2,
                    Question = "Waarom vind je deze plek het mooist?",
                    SideQuestionOptions = new List<SideQuestionOption>(),
                    SideAnswers = new List<SideAnswer>()
                };
                photoQuestion2.SideQuestions.Add(sideQuestion2);

                var sideQuestion3 = new SideQuestion()
                {
                    PhotoQuestion = photoQuestion1,
                    Question = "Waarom bezoek je deze plek het vaakst?",
                    SideAnswers = new List<SideAnswer>()
                };
                photoQuestion1.SideQuestions.Add(sideQuestion3);

                #endregion

                #region SideQuestionOptions

                var option0 = new SideQuestionOption()
                {
                    Value = "Het uitzicht is heel mooi.",
                    MultipleChoiceSideQuestion = sideQuestion2
                };
                sideQuestion2.SideQuestionOptions.Add(option0);

                var option1 = new SideQuestionOption()
                {
                    Value = "Ik kan hier tot rust komen.",
                    MultipleChoiceSideQuestion = sideQuestion2
                };
                sideQuestion2.SideQuestionOptions.Add(option1);

                var option2 = new SideQuestionOption()
                {
                    Value = "Ik heb een bijzondere band met deze plek.",
                    MultipleChoiceSideQuestion = sideQuestion2
                };
                sideQuestion2.SideQuestionOptions.Add(option2);

                var option4 = new SideQuestionOption()
                {
                    Value = "Ik heb een andere reden.",
                    MultipleChoiceSideQuestion = sideQuestion2
                };
                sideQuestion2.SideQuestionOptions.Add(option4);

                #endregion

                #region Teacher

                UserAccount account1 = new UserAccount()
                {
                    UserName = "Johannes_The_Rich##" + setUp1.loginIndentifier,
                    Email = "JohannesTheRich@kdg.be",
                    EmailConfirmed = true,
                };
                var teacher1pwd = "123Rich321";
                Teacher teacher1 = new Teacher()
                {
                    UserAccount = account1,
                    TeacherTasks = new List<TeacherTask>(),
                    Groups = new List<Group>(),
                    IsGuest = false,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    SetUp = setUp1,
                    FilterProfiles = new List<FilterProfile>()
                };
                setUp1.Teachers.Add(teacher1);
                teacher1.TeacherTasks.Add(teacherTask);
                teacherTask.Teacher = teacher1;

                result = userManager.CreateAsync(account1, teacher1pwd).Result;
                UserAccount account2 = new UserAccount()
                {
                    UserName = "Johannes_The_Poor##" + setUp1.loginIndentifier,
                    Email = "JohannesThePoor@kdg.be",
                    EmailConfirmed = true,
                };
                var teacher2pwd = "123Poor321";
                Teacher teacher2 = new Teacher()
                {
                    UserAccount = account2,
                    TeacherTasks = new List<TeacherTask>(),
                    Groups = new List<Group>(),
                    IsGuest = false,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    SetUp = setUp1
                };
                setUp1.Teachers.Add(teacher2);
                account1.User = teacher1;
                account2.User = teacher2;

                #endregion

                #region Group

                Group group1 = new Group()
                {
                    GroupCode = "123ABC",
                    Name = "3de jaar Techniek Wetenschappen",
                    Tasks = new List<GroupTask>(),
                    Teacher = teacher1,
                    AcceptDeliveries = true,
                    TaskDeliveries = new List<TaskDelivery>(),
                };

                Group group2 = new Group()
                {
                    GroupCode = "456DEF",
                    Name = "1ste jaar Moderne Wetenschappen",
                    Tasks = new List<GroupTask>(),
                    Teacher = teacher1,
                    TaskDeliveries = new List<TaskDelivery>(),
                    AcceptDeliveries = true,
                    Active = true
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
                group1.Tasks.Add(groupTask1);
                setTask1.Groups.Add(groupTask1);
                GroupTask groupTask2 = new GroupTask()
                {
                    Task = setTask2,
                    Group = group2
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
                    SetUp = setUp1
                };

                GroupInfo groupInfo2 = new GroupInfo()
                {
                    Group = group2,
                    SetUp = setUp1
                };
                group1.Info = groupInfo1;
                group2.Info = groupInfo2;

                #endregion

                #endregion

                #region TaskDelivery

                SetTaskDelivery taskDelivery1 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false
                };
                SetTaskDelivery taskDelivery2 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false
                };
                SetTaskDelivery taskDelivery3 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group1,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask2,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false
                };
                SetTaskDelivery taskDelivery4 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group1,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask2,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false
                };
                SetTaskDelivery taskDelivery5 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };
                SetTaskDelivery taskDelivery6 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };
                SetTaskDelivery taskDelivery7 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };
                SetTaskDelivery taskDelivery8 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };

                SetTaskDelivery taskDelivery9 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };
                
                
                SetTaskDelivery taskDelivery10 = new SetTaskDelivery()
                {
                    Answers = new List<Answer>(),
                    Group = group2,
                    SentPhotos = new List<Photo>(),
                    SetTask = setTask1,
                    Tags = new List<Tag>(),
                    Finished = true,
                    IsPublished = false

                };

                

                #region photo

                Photo photo1 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery1,
                    Picture = "groenplaats.jpg"
                };


                Photo photo2 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery1,
                    Picture = "wagamama.jpg"
                };

                Photo photo3 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery1,
                    Picture = "station.jpg"
                };
                Photo photo4 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery2,
                    Picture = "meir.jpg"
                };
                Photo photo5 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery2,
                    Picture = "trix.jpg"
                };
                Photo photo6 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery3,
                    Picture = "mixen.jpg"
                };
                Photo photo7 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery3,
                    Picture = "ballet.jpg"
                };
                Photo photo8 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery3,
                    Picture = "guitar.jpg"
                };
                Photo photo9 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery4,
                    Picture = "drummen.jpg"
                };
                Photo photo10 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery1,
                    Picture = "gamen.jpg"
                };
                Photo photo13 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery5,
                    Picture = "Blikfabriek.jpg"
                };
                Photo photo14 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery5,
                    Picture = "Brug.jpg"
                };
                Photo photo15 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery6,
                    Picture = "TrixZaal.jpg"
                }; 
                Photo photo16 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery6,
                    Picture = "Park_Spoor_Noord.jpg"
                };

                Photo photo17 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery7,
                    Picture = "Plantentuin.jpg"
                };
                Photo photo18 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery8,
                    Picture = "Kaaien.jpg"
                };
                Photo photo19 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery9,
                    Picture = "Kinepolis.jpg"
                };
                Photo photo20 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery9,
                    Picture = "Zoo.jpg"
                };
                Photo photo21 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery9,
                    Picture = "Mas.jpg"
                };
                Photo photo22 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery9,
                    Picture = "Centraal_Station.jpg"
                    
                };

                Photo photo23 = new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery9,
                    Picture = "Kot.jpg"
                };
                
                Photo photo24= new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery10,
                    Picture = "VoetgangersTunnel.jpg"
                };
                Photo photo25= new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery10,
                    Picture = "Aquatopia.jpg"
                };
                
                
                Photo photo26= new Photo()
                {
                    Answers = new List<Answer>(),
                    TaskDelivery = taskDelivery10,
                    Picture = "Grote_Markt_Antwerpen.jpg"
                };
                

                
                


                taskDelivery1.SentPhotos.Add(photo1);
                taskDelivery1.SentPhotos.Add(photo2);
                taskDelivery1.SentPhotos.Add(photo3);
                taskDelivery2.SentPhotos.Add(photo4);
                taskDelivery2.SentPhotos.Add(photo5);
                taskDelivery3.SentPhotos.Add(photo6);
                taskDelivery3.SentPhotos.Add(photo7);
                taskDelivery3.SentPhotos.Add(photo8);
                taskDelivery4.SentPhotos.Add(photo9);
                taskDelivery4.SentPhotos.Add(photo10);
                
                taskDelivery5.SentPhotos.Add(photo13);
                taskDelivery5.SentPhotos.Add(photo14);
                taskDelivery6.SentPhotos.Add(photo15);
                taskDelivery6.SentPhotos.Add(photo16);
                taskDelivery7.SentPhotos.Add(photo17);
                taskDelivery8.SentPhotos.Add(photo18);
                taskDelivery9.SentPhotos.Add(photo19);
                taskDelivery9.SentPhotos.Add(photo20);
                taskDelivery9.SentPhotos.Add(photo21);
                taskDelivery9.SentPhotos.Add(photo22);
                taskDelivery9.SentPhotos.Add(photo23);
                taskDelivery10.SentPhotos.Add(photo24);
                taskDelivery10.SentPhotos.Add(photo25);
                taskDelivery10.SentPhotos.Add(photo26);

                var imageFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");

                try
                {
                    storage.UploadImage(imageFolder + "/" + photo1.Picture, photo1.Picture);
                    storage.UploadImage(imageFolder + "/" + photo2.Picture, photo2.Picture);
                    storage.UploadImage(imageFolder + "/" + photo3.Picture, photo3.Picture);
                    storage.UploadImage(imageFolder + "/" + photo4.Picture, photo4.Picture);
                    storage.UploadImage(imageFolder + "/" + photo5.Picture, photo5.Picture);
                    storage.UploadImage(imageFolder + "/" + photo6.Picture, photo6.Picture);
                    storage.UploadImage(imageFolder + "/" + photo7.Picture, photo7.Picture);
                    storage.UploadImage(imageFolder + "/" + photo8.Picture, photo8.Picture);
                    storage.UploadImage(imageFolder + "/" + photo9.Picture, photo9.Picture);
                    storage.UploadImage(imageFolder + "/" + photo10.Picture, photo10.Picture);
                    storage.UploadImage(imageFolder + "/" + photo11.Picture, photo11.Picture);
                    storage.UploadImage(imageFolder + "/" + photo13.Picture, photo13.Picture);
                    storage.UploadImage(imageFolder + "/" + photo14.Picture, photo14.Picture);
                    storage.UploadImage(imageFolder + "/" + photo15.Picture, photo15.Picture);
                    storage.UploadImage(imageFolder + "/" + photo16.Picture, photo16.Picture);
                    storage.UploadImage(imageFolder + "/" + photo17.Picture, photo17.Picture);
                    storage.UploadImage(imageFolder + "/" + photo18.Picture, photo18.Picture);
                    storage.UploadImage(imageFolder + "/" + photo19.Picture, photo19.Picture);
                    storage.UploadImage(imageFolder + "/" + photo20.Picture, photo20.Picture);
                    storage.UploadImage(imageFolder + "/" + photo21.Picture, photo21.Picture);
                    storage.UploadImage(imageFolder + "/" + photo22.Picture, photo22.Picture);
                    storage.UploadImage(imageFolder + "/" + photo23.Picture, photo23.Picture);
                    storage.UploadImage(imageFolder + "/" + photo24.Picture, photo24.Picture);
                    storage.UploadImage(imageFolder + "/" + photo25.Picture, photo25.Picture);
                    storage.UploadImage(imageFolder + "/" + photo26.Picture, photo26.Picture);


                    //this section makes sure to upload the gifs we use in the apllication to the storage bu 
                    var studentGifFolder = Path.Combine(webHostEnvironment.WebRootPath, "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/1.gif", "1.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/2.gif", "2.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/3.gif", "3.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/4.gif", "4.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/5.gif", "5.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/6.gif", "6.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/7.gif", "7.gif", "gifs/student/");
                    storage.UploadImage(studentGifFolder + "/8.gif", "8.gif", "gifs/student/");

                    var adminGifFolder = Path.Combine(webHostEnvironment.WebRootPath, "gifs/admin/");
                    storage.UploadImage(adminGifFolder + "/1.gif", "1.gif", "gifs/admin/");
                    storage.UploadImage(adminGifFolder + "/2.gif", "2.gif", "gifs/admin/");
                    storage.UploadImage(adminGifFolder + "/3.gif", "3.gif", "gifs/admin/");
                    storage.UploadImage(adminGifFolder + "/4.gif", "4.gif", "gifs/admin/");
                    storage.UploadImage(adminGifFolder + "/5.gif", "5.gif", "gifs/admin/");
                }
                catch (Exception e)
                {
                    var ex = e;
                }

                #endregion

                #region answers

                var answer1 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo1,
                    Flagged = false,
                    IsQuarantined = true,
                    QualityScore = -50,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery1
                };
                photo1.Answers.Add(answer1);
                photoQuestion2.Answers.Add(answer1);
                taskDelivery1.Answers.Add(answer1);

                var answer2 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo2,
                    Flagged = false,
                    IsQuarantined = true,
                    QualityScore = -50,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery1
                };
                photo2.Answers.Add(answer2);
                photoQuestion1.Answers.Add(answer2);
                taskDelivery1.Answers.Add(answer2);

                var answer3 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo3,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery1
                };
                photo3.Answers.Add(answer3);
                photoQuestion2.Answers.Add(answer3);
                taskDelivery1.Answers.Add(answer3);

                var answer4 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo4,
                    Flagged = true,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery2
                };
                photo4.Answers.Add(answer4);
                photoQuestion1.Answers.Add(answer4);
                taskDelivery2.Answers.Add(answer4);

                var answer5 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo5,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery2
                };
                photo5.Answers.Add(answer5);
                photoQuestion1.Answers.Add(answer5);
                taskDelivery2.Answers.Add(answer5);

                var answer6 = new Answer()
                {
                    AnsweredQuestion = photoQuestion3,
                    AssignedPhoto = photo6,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery3
                };
                photo6.Answers.Add(answer6);
                photoQuestion3.Answers.Add(answer6);
                taskDelivery3.Answers.Add(answer6);

                var answer7 = new Answer()
                {
                    AnsweredQuestion = photoQuestion3,
                    AssignedPhoto = photo7,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery3
                };
                photo7.Answers.Add(answer7);
                photoQuestion3.Answers.Add(answer7);
                taskDelivery3.Answers.Add(answer7);

                var answer8 = new Answer()
                {
                    AnsweredQuestion = photoQuestion3,
                    AssignedPhoto = photo8,
                    Flagged = true,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery4
                };
                photo8.Answers.Add(answer8);
                photoQuestion3.Answers.Add(answer8);
                taskDelivery4.Answers.Add(answer8);

                var answer9 = new Answer()
                {
                    AnsweredQuestion = photoQuestion3,
                    AssignedPhoto = photo9,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery4
                };
                photo9.Answers.Add(answer9);
                photoQuestion3.Answers.Add(answer9);
                taskDelivery4.Answers.Add(answer9);

                var answer10 = new Answer()
                {
                    AnsweredQuestion = photoQuestion3,
                    AssignedPhoto = photo10,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery4
                };
                photo10.Answers.Add(answer10);
                photoQuestion3.Answers.Add(answer10);
                taskDelivery4.Answers.Add(answer10);
                
                
                var answer11 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo13,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery5
                };
                photo13.Answers.Add(answer11);
                photoQuestion1.Answers.Add(answer11);
                taskDelivery5.Answers.Add(answer11);
                
                var answer12 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo14,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery5
                };
                photo14.Answers.Add(answer12);
                photoQuestion1.Answers.Add(answer12);
                taskDelivery5.Answers.Add(answer12);
                
                var answer13 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo15,
                    Flagged = true,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery6
                };
                photo15.Answers.Add(answer13);
                photoQuestion1.Answers.Add(answer13);
                taskDelivery6.Answers.Add(answer13);
                
                
                var answer14 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo16,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery6
                };
                photo16.Answers.Add(answer14);
                photoQuestion2.Answers.Add(answer14);
                taskDelivery6.Answers.Add(answer14);
                
                var answer15 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo17,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery7
                };
                photo17.Answers.Add(answer15);
                photoQuestion2.Answers.Add(answer15);
                taskDelivery7.Answers.Add(answer15);
                
                var answer16 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo18,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery8
                };
                photo18.Answers.Add(answer16);
                photoQuestion1.Answers.Add(answer16);
                taskDelivery8.Answers.Add(answer16);
                
                var answer17 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo19,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery9
                };
                photo19.Answers.Add(answer17);
                photoQuestion1.Answers.Add(answer17);
                taskDelivery9.Answers.Add(answer17);
                
                var answer18 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo20,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery9
                };
                photo20.Answers.Add(answer18);
                photoQuestion2.Answers.Add(answer18);
                taskDelivery9.Answers.Add(answer18);
                
                var answer19 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo21,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery9
                };
                photo21.Answers.Add(answer19);
                photoQuestion2.Answers.Add(answer19);
                taskDelivery9.Answers.Add(answer19);
                
                var answer20 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo22,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery9
                };
                photo22.Answers.Add(answer20);
                photoQuestion2.Answers.Add(answer20);
                taskDelivery9.Answers.Add(answer20);
                
                var answer21 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo23,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery9
                };
                photo23.Answers.Add(answer21);
                photoQuestion1.Answers.Add(answer21);
                taskDelivery9.Answers.Add(answer21);
                
                var answer22 = new Answer()
                {
                    AnsweredQuestion = photoQuestion1,
                    AssignedPhoto = photo24,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery10
                };
                photo24.Answers.Add(answer22);
                photoQuestion1.Answers.Add(answer22);
                taskDelivery10.Answers.Add(answer22);
                
                var answer23 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo25,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery10
                };
                photo25.Answers.Add(answer23);
                photoQuestion2.Answers.Add(answer23);
                taskDelivery10.Answers.Add(answer23);
                
                var answer24 = new Answer()
                {
                    AnsweredQuestion = photoQuestion2,
                    AssignedPhoto = photo26,
                    Flagged = false,
                    IsQuarantined = false,
                    QualityScore = 0,
                    QualityScoreEvents = new List<QualityScoreEvent>(),
                    CustomTags = new List<Tag>(),
                    SideAnswers = new List<SideAnswer>(),
                    TaskDelivery = taskDelivery10
                };
                photo26.Answers.Add(answer24);
                photoQuestion2.Answers.Add(answer24);
                taskDelivery10.Answers.Add(answer24);
                
                

                #endregion

                #region sideAnswers

                var sideAnswer1 = new SideAnswer()
                {
                    Answer = answer1,
                    AnsweredQuestion = sideQuestion2,
                    GivenAnswer = "Het uitzicht is heel mooi."
                };
                answer1.SideAnswers.Add(sideAnswer1);
                sideQuestion2.SideAnswers.Add(sideAnswer1);

                var sideAnswer2 = new SideAnswer()
                {
                    Answer = answer2,
                    AnsweredQuestion = sideQuestion1,
                    GivenAnswer = "3"
                };
                answer2.SideAnswers.Add(sideAnswer2);
                sideQuestion1.SideAnswers.Add(sideAnswer2);

                var sideAnswer3 = new SideAnswer()
                {
                    Answer = answer2,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Ze hebben mijn favoriete maaltijd."
                };
                answer2.SideAnswers.Add(sideAnswer3);
                sideQuestion3.SideAnswers.Add(sideAnswer3);

                var sideAnswer4 = new SideAnswer()
                {
                    Answer = answer3,
                    AnsweredQuestion = sideQuestion2,
                    GivenAnswer = "Ik heb een bijzondere band met deze plek."
                };
                answer3.SideAnswers.Add(sideAnswer4);
                sideQuestion2.SideAnswers.Add(sideAnswer4);

                var sideAnswer5 = new SideAnswer()
                {
                    Answer = answer4,
                    AnsweredQuestion = sideQuestion1,
                    GivenAnswer = "4"
                };
                answer4.SideAnswers.Add(sideAnswer5);
                sideQuestion1.SideAnswers.Add(sideAnswer5);

                var sideAnswer6 = new SideAnswer()
                {
                    Answer = answer4,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Ik ga graag shoppen."
                };
                answer4.SideAnswers.Add(sideAnswer6);
                sideQuestion3.SideAnswers.Add(sideAnswer6);

                var sideAnswer7 = new SideAnswer()
                {
                    Answer = answer5,
                    AnsweredQuestion = sideQuestion1,
                    GivenAnswer = "3"
                };
                answer5.SideAnswers.Add(sideAnswer7);
                sideQuestion1.SideAnswers.Add(sideAnswer7);

                var sideAnswer8 = new SideAnswer()
                {
                    Answer = answer5,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Ik ga veel naar concerten."
                };
                answer5.SideAnswers.Add(sideAnswer8);
                sideQuestion3.SideAnswers.Add(sideAnswer8);
                
                
                var sideAnswer9 = new SideAnswer()
                {
                    Answer = answer11,
                    AnsweredQuestion = sideQuestion1,
                    GivenAnswer = "7"
                };
                answer11.SideAnswers.Add(sideAnswer9);
                sideQuestion1.SideAnswers.Add(sideAnswer9);
                
                var sideAnswer10 = new SideAnswer()
                {
                    Answer = answer11,
                    AnsweredQuestion = sideQuestion2,
                    GivenAnswer = "Ik kom hier vaak wanneer ik een fietstocht maak"
                };
                answer11.SideAnswers.Add(sideAnswer10);
                sideQuestion1.SideAnswers.Add(sideAnswer10);
                
                var sideAnswer11= new SideAnswer()
                {
                    Answer = answer12,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Ik kan hier tot rust komen."
                };
                answer12.SideAnswers.Add(sideAnswer11);
                sideQuestion2.SideAnswers.Add(sideAnswer11);
                
                var sideAnswer12= new SideAnswer()
                {
                    Answer = answer14,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Het uitzicht is heel mooi."
                };
                answer14.SideAnswers.Add(sideAnswer12);
                sideQuestion3.SideAnswers.Add(sideAnswer12);
                
                var sideAnswer13= new SideAnswer()
                {
                    Answer = answer15,
                    AnsweredQuestion = sideQuestion3,
                    GivenAnswer = "Ik heb een andere reden."
                };
                answer15.SideAnswers.Add(sideAnswer13);
                sideQuestion3.SideAnswers.Add(sideAnswer13);
                
                var sideAnswer14= new SideAnswer()
                {
                    Answer = answer15,
                    AnsweredQuestion = sideQuestion1,
                    GivenAnswer = "4"
                };
                answer16.SideAnswers.Add(sideAnswer14);
                sideQuestion1.SideAnswers.Add(sideAnswer14);
                #endregion

                #endregion

                #region Tags

                var tag11 = new Tag()
                {
                    TaskDelivery = taskDelivery1,
                    Description = "Geslacht",
                    Value = "Man"
                };
                taskDelivery1.Tags.Add(tag11);

                var tag12 = new Tag()
                {
                    TaskDelivery = taskDelivery1,
                    Description = "Leeftijd",
                    Value = "12"
                };
                taskDelivery1.Tags.Add(tag12);

                var tag21 = new Tag()
                {
                    TaskDelivery = taskDelivery2,
                    Description = "Geslacht",
                    Value = "Vrouw"
                };
                taskDelivery2.Tags.Add(tag21);

                var tag22 = new Tag()
                {
                    TaskDelivery = taskDelivery2,
                    Description = "Leeftijd",
                    Value = "13"
                };
                taskDelivery2.Tags.Add(tag22);

                var tag31 = new Tag()
                {
                    TaskDelivery = taskDelivery3,
                    Description = "Geslacht",
                    Value = "Vrouw"
                };
                taskDelivery3.Tags.Add(tag31);

                var tag32 = new Tag()
                {
                    TaskDelivery = taskDelivery3,
                    Description = "Leeftijd",
                    Value = "12"
                };
                taskDelivery3.Tags.Add(tag32);

                var tag41 = new Tag()
                {
                    TaskDelivery = taskDelivery4,
                    Description = "Geslacht",
                    Value = "Man"
                };
                taskDelivery4.Tags.Add(tag41);

                var tag42 = new Tag()
                {
                    TaskDelivery = taskDelivery4,
                    Description = "Leeftijd",
                    Value = "13"
                };
                taskDelivery4.Tags.Add(tag42);
                
                var tag43 = new Tag()
                {
                    TaskDelivery = taskDelivery6,
                    Description = "Leeftijd",
                    Value = "22"
                };
                taskDelivery5.Tags.Add(tag43);
                var tag44 = new Tag()
                {
                    TaskDelivery = taskDelivery5,
                    Description = "Geslacht",
                    Value = "Man"
                };
                taskDelivery5.Tags.Add(tag44);
                
                var tag45 = new Tag()
                {
                    TaskDelivery = taskDelivery6,
                    Description = "Geslacht",
                    Value = "Andere"
                };
                taskDelivery6.Tags.Add(tag45);
                var tag46 = new Tag()
                {
                    TaskDelivery = taskDelivery6,
                    Description = "Leeftijd",
                    Value = "22"
                };
                taskDelivery6.Tags.Add(tag46);
                var tag47 = new Tag()
                {
                    TaskDelivery = taskDelivery7,
                    Description = "Geslacht",
                    Value = "Andere"
                };
                taskDelivery7.Tags.Add(tag47);
                var tag48 = new Tag()
                {
                    TaskDelivery = taskDelivery7,
                    Description = "Leeftijd",
                    Value = "22"
                };
                taskDelivery7.Tags.Add(tag48);
                
                var tag49 = new Tag()
                {
                    TaskDelivery = taskDelivery8,
                    Description = "Geslacht",
                    Value = "Vrouw"
                };
                taskDelivery8.Tags.Add(tag49);
                var tag50 = new Tag()
                {
                    TaskDelivery = taskDelivery8,
                    Description = "Leeftijd",
                    Value = "21"
                };
                taskDelivery8.Tags.Add(tag50);
                
                var tag51 = new Tag()
                {
                    TaskDelivery = taskDelivery9,
                    Description = "Geslacht",
                    Value = "Vrouw"
                };
                taskDelivery9.Tags.Add(tag51);
                var tag52 = new Tag()
                {
                    TaskDelivery = taskDelivery9,
                    Description = "Leeftijd",
                    Value = "21"
                };
                taskDelivery9.Tags.Add(tag52);
                
                var tag53 = new Tag()
                {
                    TaskDelivery = taskDelivery9,
                    Description = "Geslacht",
                    Value = "Man"
                };
                taskDelivery10.Tags.Add(tag53);
                var tag54 = new Tag()
                {
                    TaskDelivery = taskDelivery9,
                    Description = "Leeftijd",
                    Value = "21"
                };
                taskDelivery10.Tags.Add(tag54);

                #endregion

                var gInfo1 = new GroupInfo()
                {
                    Group = group1,
                    TeacherName = group1.Teacher.UserAccount.UserName,
                    TotalPhotos = 13,
                    TotalStudents = 13
                };
                
                

                setUp1.Usage.Add(gInfo1);

                group2.TaskDeliveries.Add(taskDelivery1);
                group2.TaskDeliveries.Add(taskDelivery2);
                group1.TaskDeliveries.Add(taskDelivery3);
                group1.TaskDeliveries.Add(taskDelivery4);
                group2.TaskDeliveries.Add(taskDelivery5);
                group2.TaskDeliveries.Add(taskDelivery6);
                group2.TaskDeliveries.Add(taskDelivery7);
                group2.TaskDeliveries.Add(taskDelivery8);
                group2.TaskDeliveries.Add(taskDelivery9);
                group2.TaskDeliveries.Add(taskDelivery10);

                setTask1.SetTaskDeliveries.Add(taskDelivery1);
                setTask1.SetTaskDeliveries.Add(taskDelivery2);
                setTask2.SetTaskDeliveries.Add(taskDelivery3);
                setTask2.SetTaskDeliveries.Add(taskDelivery4);
                setTask1.SetTaskDeliveries.Add(taskDelivery5);
                setTask1.SetTaskDeliveries.Add(taskDelivery6);
                setTask1.SetTaskDeliveries.Add(taskDelivery7);
                setTask1.SetTaskDeliveries.Add(taskDelivery8);
                setTask1.SetTaskDeliveries.Add(taskDelivery9);
                setTask1.SetTaskDeliveries.Add(taskDelivery10);
                

                #region FilterProfiles

                var filterProfile1 = new FilterProfile()
                {
                    ProfileName = "Mannen van 22",
                    User = teacher1,
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            Description = "Geslacht",
                            Value = "Man"
                        },
                        new Filter()
                        {
                            Description = "Leeftijd",
                            Value = "22"
                        }
                    }
                };

                var filterProfile2 = new FilterProfile()
                {
                    ProfileName = "Vrouwen van 21",
                    User = teacher1,
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            Description = "Geslacht",
                            Value = "Vrouw"
                        },
                        new Filter()
                        {
                            Description = "Leeftijd",
                            Value = "21"
                        }
                    }
                };

                var filterProfile3 = new FilterProfile()
                {
                    ProfileName = "Vrouwen",
                    User = teacher1,
                    Filters = new List<Filter>()
                    {
                        new Filter()
                        {
                            Description = "Geslacht",
                            Value = "Vrouw"
                        }
                    }
                };

                teacher1.FilterProfiles.Add(filterProfile1);
                teacher1.FilterProfiles.Add(filterProfile2);
                teacher1.FilterProfiles.Add(filterProfile3);
                var gInfo2 = new GroupInfo()
                {
                    Group = group2,
                    TeacherName = group2.Teacher.UserAccount.UserName,
                    TotalPhotos = 69,
                    TotalStudents = 69
                };

                setUp1.Usage.Add(gInfo2);

                #endregion


                result = userManager.CreateAsync(account2, teacher2pwd).Result;
                /* }
     
                 if (!userManager
                     .IsInRoleAsync(userManager.FindByEmailAsync("Daniel.Deboeverie@gmail.com").Result, "Admin").Result)
                 {*/

                result = userManager.AddToRoleAsync(adminUserAccount, "ADMIN").Result;
                result = userManager.AddToRoleAsync(adminUserAccount2, "ADMIN").Result;
                result = userManager.AddToRoleAsync(adminUserAccount3, "ADMIN").Result;
                result = userManager.AddToRoleAsync(adminUserAccount4, "ADMIN").Result;


                result = userManager.AddToRoleAsync(account1, "Teacher").Result;


                result = userManager.AddToRoleAsync(account2, "Teacher").Result;
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role1 = new IdentityRole();
                role1.Name = "Admin";
                var result = roleManager.CreateAsync(role1).Result;

                var role2 = new IdentityRole();
                role2.Name = "Teacher";
                result = roleManager.CreateAsync(role2).Result;

                var role3 = new IdentityRole();
                role3.Name = "GuestTeacher";
                result = roleManager.CreateAsync(role3).Result;
            }
        }
    }
}