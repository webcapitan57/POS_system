using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.profile;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using QRCoder;
using UI_MVC.Gcloud;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly ITeacherService _teacherService;
        private readonly ITaskService _taskService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly GcloudOptions _options;

        public GroupController(IGroupService groupService,
            IStudentService studentService, ITeacherService teacherService,
            ITaskService taskService, UserManager<UserAccount> userManager,
            IEmailSender emailSender,IOptions<GcloudOptions> options)
        
        {
            _groupService = groupService;
            _studentService = studentService;
            _teacherService = teacherService;
            _taskService = taskService;
            _userManager = userManager;
            _emailSender = emailSender;
            _options = options.Value;
        }

        public IActionResult Join()
        {
            var model = new JoinGroupViewModel()
            {
                ValidationExceptions = new List<ValidationException>()
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Join(JoinGroupViewModel model)
        {
            if (model.GroupCode == null)
            {
                return View(model);
            }

            var currentGroup = _groupService.GetGroupWithTeacher(model.GroupCode.ToUpper());

            if (currentGroup == null)
            {
                model.ValidationExceptions.Add(new ValidationException("Geef aub een bestaande groepscode in."));
                return View(model);
            }

            if (!currentGroup.Active)
            {
                model.ValidationExceptions.Add(new ValidationException("Deze groep is niet actief."));
                return View(model);
            }

            if (currentGroup.HasLimit && currentGroup.Info.TotalStudents >= currentGroup.MaxParticipants)
            {
                model.ValidationExceptions.Add(new ValidationException("Deze groep is volzet."));
                return View(model);
            }

            var newStudent = _studentService.AddStudent(currentGroup);
            _groupService.UpdateTotalStudentsOfGroupInfo(currentGroup);
            var profileQuestions = GetOpenProfileQuestions(currentGroup, newStudent);

            _studentService.UpdateStudent(newStudent);

            return profileQuestions.ToList().Count == 0
                ? RedirectToAction("Show", "Task",
                    new {groupCode = currentGroup.GroupCode, studentId = newStudent.StudentId})
                : RedirectToAction("Profile",
                    new {groupCode = currentGroup.GroupCode, studentId = newStudent.StudentId});
        }

        [HttpGet]
        public IActionResult Profile(string groupCode, int studentId)
        {
            var group = _groupService.GetGroupWithTeacher(groupCode.ToUpper());
            var student = _studentService.GetStudentWithProfileAnswers(studentId);

            var questions = GetOpenProfileQuestions(group, student);
            var viewModel = new ProfileViewModel()
            {
                Group = group,
                Student = student,
                Answers = questions
                    .Select(question => new StudentProfileAnswer()
                    {
                        AnsweredQuestion = question
                    }).ToList()
            };
            
            ViewBag.bucketName = _options.BucketName;
            return View(viewModel);
        }

        // [Authorize(Policy = "TeacherOrHigher")]
        public IActionResult Index()
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherWithGroupsAndTasks(userAccount.Id);
            _taskService.CheckEmptyTasks(teacher);

            ViewBag.bucketName = _options.BucketName;
            return View(teacher);
        }

        //  [Authorize(Policy = "GuestTeacherOrHigher")]
        [HttpGet]
        public IActionResult Create()
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacher(userAccount.Id);

            var questions = _groupService.GetGroupProfileQuestions(teacher.SetUp.SetUpId).ToList();
            var viewModel = new CreateGroupViewModel()
            {
                Setup = teacher.SetUp,
                SetUpId = teacher.SetUp.SetUpId,
                Answers = questions
                    .Select(question => new GroupProfileAnswer()
                    {
                        AnsweredQuestion = question
                    }).ToList()
            };
            ViewBag.bucketName = _options.BucketName;
            return View(viewModel);
        }

        //   [Authorize(Policy = "GuestTeacherOrHigher")]
        [HttpPost]
        public IActionResult Create(CreateGroupViewModel createGroup)
        {
            if (createGroup.Answers != null)
            {
                foreach (var profileAnswer in createGroup.Answers)
                {
                    profileAnswer.AnsweredQuestion =
                        _groupService.GetGroupProfileQuestions(createGroup.SetUpId)
                            .FirstOrDefault(
                                q => q.ProfileQuestionId == profileAnswer.AnsweredQuestion.ProfileQuestionId)!;
                }
            }

            //validating the group 
            createGroup.Errors ??= new List<ValidationResult>();
            Validator.TryValidateObject(createGroup, new ValidationContext(createGroup), createGroup.Errors, true);

            if (createGroup.Errors.Count != 0)
            {
                return View(createGroup);
            }

            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacherWithGroups(userAccount.Id);
            var newGroup = _groupService.AddGroup(createGroup.SetUpId, createGroup.GroupName, teacher,
                GenerateRandomGroupCode(), createGroup.MaxParticipants);
            /*_groupService.AddGroupProfileAnswers(newGroup, createGroup.GroupProfileQuestions.ToList(),
                createGroup.Answers.ToList());*/
            if (_userManager.IsInRoleAsync(userAccount, "Teacher").Result)
            {
                var link = HttpContext.Request.IsHttps ? "https://" : "http://";
                link += HttpContext.Request.Host.Value;

                _emailSender.SendEmailAsync(userAccount.Email, "CreateGroup",
                    $"{newGroup.Name},{newGroup.GroupCode},{link}/Group/StartSession?groupCode={newGroup.GroupCode}");
            }

            if (_userManager.IsInRoleAsync(userAccount, "GuestTeacher").Result)
            {
                _userManager.AddPasswordAsync(userAccount, createGroup.Password);
            }

            return RedirectToAction("Tasks", new {groupCode = newGroup.GroupCode});
        }


        // [Authorize(Policy = "GuestTeacherOrHigher")]
        public IActionResult Tasks(string groupCode)
        {
            var viewModel = MakeEditGroupTaskViewModel(groupCode);

            ViewBag.bucketName = _options.BucketName;
            return View(viewModel);
        }

        public IActionResult Remove(string groupCode)
        {
            var group = _groupService.GetGroupWithTeacherAndTasks(groupCode);
            ViewBag.bucketName = _options.BucketName;
            return View(group);
        }

        [HttpPost]
        public IActionResult RemoverGroupConfirmed(string groupCode)
        {
            // Code here wil get The Teacher Id of the Teacher that has sent the request
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacher(userAccount.Id);
            _groupService.RemoveGroup(groupCode);
            return RedirectToAction("Index");
        }

        // [Authorize(Policy = "GuestTeacherOrHigher")]
        public IActionResult Edit(string groupCode)
        {
            var group = _groupService.GetGroupWithTeacherAndTasks(groupCode);
            var setTasks = _taskService.GetSetTasksOfSetup(group.Teacher.SetUp.SetUpId).ToList();
            var teacherTasks = _taskService.GetTeacherTasksOfTeacher(group.Teacher.UserId).ToList();
            var alreadyAddedTasks = group.Tasks.Select(t => t.Task).ToList();

            var viewModel = new EditGroupTaskViewModel()
            {
                CurrentGroup = group,
                SetTasks = setTasks,
                TeacherTasks = teacherTasks,
                AlreadyAddedTasks = alreadyAddedTasks
            };
            EditGroupViewModel editGroupView = new EditGroupViewModel()
            {
                GroupName = group.Name,
                MaxParticipants = group.MaxParticipants,
                EditGroupTaskViewModel = viewModel,
            };
            ViewBag.bucketName = _options.BucketName;
            return View(editGroupView);
        }

        //   [Authorize(Policy = "GuestTeacherOrHigher")]
        [HttpPost]
        public IActionResult Edit(EditGroupViewModel editGroup)
        {
            if (!ModelState.IsValid)
            {
                var editGroupTaskViewModel = MakeEditGroupTaskViewModel(editGroup.groupCode);
                editGroup.EditGroupTaskViewModel = editGroupTaskViewModel;
                return View(editGroup);
            }

            var groupToEdit = _groupService.GetGroup(editGroup.groupCode);
            groupToEdit.Name = editGroup.GroupName;
            groupToEdit.MaxParticipants = editGroup.MaxParticipants;
            groupToEdit.HasLimit = groupToEdit.MaxParticipants.HasValue;
            _groupService.ChangeGroup(groupToEdit);

            //Teacher Id from the request
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var teacher = _teacherService.GetTeacher(userAccount.Id);
            return User.IsInRole("GuestTeacher") ? RedirectToAction("SingleGroup", new {editGroup.groupCode}) : RedirectToAction("Index");
        }

        // [Authorize(Policy = "GuestTeacherOrHigher")]
        public EditGroupTaskViewModel MakeEditGroupTaskViewModel(string groupCode)
        {
            var group = _groupService.GetGroupWithTeacherAndTasks(groupCode);
            var setTasks = _taskService.GetSetTasksOfSetup(group.Teacher.SetUp.SetUpId).ToList();
            var teacherTasks = _taskService.GetTeacherTasksOfTeacher(group.Teacher.UserId).ToList();
            var alreadyAddedTasks = group.Tasks.Select(t => t.Task).ToList();

            var viewModel = new EditGroupTaskViewModel()
            {
                CurrentGroup = group,
                SetTasks = setTasks,
                TeacherTasks = teacherTasks,
                AlreadyAddedTasks = alreadyAddedTasks
            };
            return viewModel;
        }

        [HttpPost]
        public IActionResult Profile(ProfileViewModel profileViewModel)
        {
            profileViewModel.Validate(null!);

            if (profileViewModel.Errors.Count != 0)
            {
                foreach (var answer in profileViewModel.Answers)
                {
                    answer.AnsweredQuestion =
                        _groupService.GetStudentProfileQuestion(answer.AnsweredQuestion.ProfileQuestionId);
                }

                return View(profileViewModel);
            }

            var student = _studentService.GetStudentWithTags(profileViewModel.Student.StudentId);

            foreach (var answer in profileViewModel.Answers)
            {
                Response.Cookies.Append(answer.AnsweredQuestion.ProfileQuestionId.ToString(), answer.Value,
                    new CookieOptions {Expires = DateTimeOffset.MaxValue}
                );
                student.ProfileAnswers.Add(answer);
            }

            _studentService.UpdateStudent(student);

            return RedirectToAction("show", "Task",
                new
                {
                    groupCode = profileViewModel.Group.GroupCode,
                    studentId = profileViewModel.Student.StudentId,
                    firstRun = true
                });
        }

        public List<StudentProfileQuestion> GetOpenProfileQuestions(Group group, Student student)
        {
            var cookies = new List<KeyValuePair<string, string>>();

            foreach (var key in Request.Cookies.Keys)
            {
                Request.Cookies.TryGetValue(key, out var value);
                cookies.Add(new KeyValuePair<string, string>(key, value));
            }

            var questions = _groupService.GetOpenProfileQuestions(student, group, cookies).ToList();

            return questions;
        }

        [HttpGet("{groupCode}")]
        public IActionResult SingleGroup(string groupCode)
        {
            Group group;
            if (groupCode.Equals("FindForMePls"))
            {
                var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
                group = _groupService.GetTeacherGroups(user.Id).FirstOrDefault();
                
                if (group == null)
                {
                    ViewBag.bucketName = _options.BucketName;
                    return RedirectToAction("Create");
                }
            }
            else
            {
                group = _groupService.GetGroup(groupCode);
            }

            ViewBag.bucketName = _options.BucketName;
            return View(group);
        }

        public IActionResult Help()
        {
            return View();
        }

        private byte[] BitmapToBytes(Bitmap img)
        {
            using var stream = new MemoryStream();
            
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream.ToArray();
        }

        public IActionResult StartSession(string groupCode)
        {
            var group = _groupService.GetGroup(groupCode);
            
            

            group.Active = true;
            _groupService.UpdateGroup(group);

            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(groupCode,
                QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCode(qrCodeData);

            var qrCodeImage = qrCode.GetGraphic(20);
            ViewBag.QRCode = BitmapToBytes(qrCodeImage);

            ViewBag.bucketName = _options.BucketName;
            return View(group);
        }

        private string GenerateRandomGroupCode()
        {
            string code;
            var unique = false;
            var random = new Random();

            do
            {
                code = "";
                for (var i = 0; i < 6; i++)
                {
                    code += random.Next(10).ToString();
                    unique = _groupService.GroupsCodeUnique(code);
                }
            } while (!unique);

            return code;
        }
    }
}