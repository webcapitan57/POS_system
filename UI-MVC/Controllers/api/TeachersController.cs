using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Controllers.api
{
    [ApiController]
    [Route("/api/teachers")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly ISetupService _setupService;
        private readonly IGroupService _groupService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;

        public TeachersController(ITeacherService teacherService, ISetupService setupService, IGroupService groupService, UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager)
        {
            _teacherService = teacherService;
            _setupService = setupService;
            _groupService = groupService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult GetTeachers()
        {
            var teachers = _teacherService.GetAllTeachersWithUserAccount();
            return Ok(teachers);
        }
        
        [HttpDelete("{teacherId}")]
        public IActionResult DeleteTeacher(int teacherId)
        {
            var teacher = _teacherService.GetTeacher(teacherId);
            _teacherService.RemoveTeacher(teacher);
            return NoContent();
        }
        
        [HttpPost("{setupId}")]
        public IActionResult CreateGroupGuest(int setupId)
        {
            var setup = _setupService.ReadSetupById(setupId);
            var teacher = new Teacher()
            {
                IsGuest = true,
                SetUp = setup,
            };
            var randomNumber = new Random();
            var userName = "Guest_" + randomNumber.Next(100000);
            var userAccount = new UserAccount()
            {
                Email = "Guest@gmail.com",
                EmailConfirmed = true,
                UserName = userName,
                User = teacher
            };
            teacher.UserAccount = userAccount;
            var result =  _userManager.CreateAsync(userAccount).Result;
            userAccount = _userManager.FindByNameAsync(userName).Result;
            _userManager.AddToRoleAsync(userAccount, "GuestTeacher");
            teacher.UserAccountId = userAccount.Id;   
            _teacherService.UpdateTeacher(teacher);
            _signInManager.SignInAsync(userAccount, false);

            return Redirect("/group/create");
        }
        
        [HttpPost("{groupCode}/{password}")]
        public IActionResult LoginGuest(string groupCode,string password)
        {
            var group = _groupService.GetGroupWithTeacher(groupCode);
            var userAccount = _userManager.FindByIdAsync(group.Teacher.UserAccountId).Result;
            _signInManager.PasswordSignInAsync(userAccount,password,false,false);

            return RedirectToAction("SingleGroup", "Group", new {groupCode});
        }
        
        [HttpGet("{username}")]
        public IActionResult CheckUser(string username)
        {
            var userAccount = _userManager.FindByNameAsync(username).Result;
            var returnValue = (userAccount != null).ToString();
            return Ok(returnValue);
        }
    }
}