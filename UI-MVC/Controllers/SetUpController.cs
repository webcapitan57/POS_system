using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    // [Authorize(Policy = "Admin")]
    [Route("SetUps")]
    public class SetUpController : Controller
    {
        private readonly ISetupService _setupService;
        private readonly IAdminService _adminService;
        private readonly ITaskService _taskService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly GcloudOptions _options;

        public SetUpController(ISetupService setupService, IAdminService adminService, ITaskService taskService,
            UserManager<UserAccount> userManager,
            IOptions<GcloudOptions> options)
        {
            _setupService = setupService;
            _adminService = adminService;
            _taskService = taskService;
            _userManager = userManager;
            _options = options.Value;
        }

        public IActionResult Index()
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            _setupService.ClearEmptySetUps();
            var setUps = _adminService.GetSetUpsOfAdmin(userAccount.Id);
            return View(setUps);
        }

        [HttpGet("{action}")]
        public IActionResult Create()
        {
            var setUp = new SetUp()
            {
                Name = "",
                GeneralText = "",
                Archived = false,
                CreationDate = DateTime.Now,
                NeededProfileQuestions = new List<ProfileQuestion>(),
                Admins = new List<SetUpAdmin>(),
                PrimColor = "#57BE62",
                SecColor = "#011435",
                loginIndentifier = GenerateLoginIdentifier(),
                AllowLocations = false
            };
            //Setting a default logo for the SetUp
            setUp.Logo = new Photo()
            {
                SetUp = setUp,
                Picture = "defaultLogo.jpg"
            };
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            setUp = _setupService.AddSetUp(setUp, _adminService.GetAdmin(userAccount.Id));
            return View(setUp);
        }

        [HttpGet("{setUpId}/{action}")]
        public IActionResult Edit(int setUpId)
        {
            _taskService.CheckEmptyTasks(setUpId);
            var setUp = _setupService.GetSetUp(setUpId);
            ViewBag.bucketName = _options.BucketName;
            return View(setUp);
        }

        [HttpGet("{setUpId}/{action}")]
        public IActionResult Details(int setUpId)
        {
            var setup = _setupService.GetSetUpDetails(setUpId);
            var link = HttpContext.Request.IsHttps ? "https://" : "http://";
            link += HttpContext.Request.Host.Value;
            link += "/Identity/Account/Login?setupCode=" + setup.loginIndentifier;
            var model = new SetupDetailsModel()
            {
                SetUp = setup,
                LoginUrl = link
            };
            return View(model);
        }

        [HttpGet("{setUpId}/{action}")]
        public IActionResult ManageAdmins(int setUpId)
        {
            var model = new AdminLinkerModel()
            {
                LinkedAdmins = new List<Admin>(),
                UnlinkedAdmins = new List<Admin>(),
                SetUpId = setUpId
            };
            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            var admins = _adminService.GetKnownAdmins(user.Id);
            foreach (var admin in admins)
            {
                if (admin.SetUps.Any(sa => sa.SetUp.SetUpId == setUpId))
                {
                    model.LinkedAdmins.Add(admin);
                }
                else
                {
                    model.UnlinkedAdmins.Add(admin);
                }
            }

            return View(model);
        }
        
        private string GenerateLoginIdentifier()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            var noNoWords = new List<string>
            {
                "fuck", "shit", "kanker", "cancer", "kut", "lul", "homo", "kkk", "hoer", "bitch", "slet", "idioot",
                "dikzak", "sukkel",
                "schijt", "tyfus", "corona", "flikker", "trut", "doos", "makak", "neger", "jood", "jezus", "mohammed",
                "autist", "neuk", "smurf", "muts", "bleekscheet",
                "etter", "janet", "pot", "bef", "ezel", "nazi", "seks", "pedo", "tiet", "ruk", "slaaf", "debiel", "ass",
                "tits", "fuck", "shit", "cunt", "mof", "zeik",
                "wip", "paling", "stink", "aap", "seks", "sex", "mietje", "pussy", "coochie", "terrorist", "foef",
                "hole", "jerk", "dick", "bitch", "idiot", "slut", "jerk", "kneegrow",
                "arse", "bastard", "hoe", "mother", "milf", "nigger", "racist", "buitenlander", "vluchteling", "jew",
                "moeder", "natnek", "christ", "islam", "hindu", "damn", "dammit", "archived"
            };
            string generatedString;
            bool unique;
            var hasNoNo = false;
            var random = new Random();

            do {
                generatedString = new string(Enumerable.Repeat(chars, 15)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
                unique = _setupService.CheckIfLoginIdentifierIsAlreadyTaken(generatedString);

                foreach (var noNoWord in noNoWords)
                {
                    if (generatedString.Contains(noNoWord.ToUpper()))
                    {
                        hasNoNo = true;
                    };
                }
                
            } while (!unique || hasNoNo);

            return generatedString;
        }
    }
}