using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.WebUtilities;
using POC.BL.Domain.user;
using POC.BL.logic.InterFaces;
using UI_MVC.Gcloud;
using UI_MVC.Models;

namespace UI_MVC.Controllers
{
    // [Authorize(Policy = "Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ITeacherService _teacherService;
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly GcloudStorage _storage;
        private readonly IDeliveryService _deliveryService;
        private readonly GcloudOptions _options;
        
        public AdminController(IAdminService adminService, ITeacherService teacherService,
            UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager, IEmailSender emailSender,
            GcloudStorage storage, IDeliveryService deliveryService, IOptions<GcloudOptions> options)
        {
            _adminService = adminService;
            _teacherService = teacherService;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _storage = storage;
            _deliveryService = deliveryService;
            _options = options.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{action}")]
        public IActionResult ModerateTeachers()
        {
            var teachers = _teacherService.GetAllTeachers();
            return View(teachers);
        }

        [HttpGet("{action}")]
        public IActionResult ModerateMarkedPhotos()
        {
            var userAccount = _userManager.FindByNameAsync(User.Identity.Name).Result;
            //var user = _userService.GetUserForSet(userAccount.Id);
            var adminSetUps = _adminService.GetSetUpsOfAdmin(userAccount.Id);
            var markedPhotos = _deliveryService.GetMarkedAnswers(adminSetUps.ToList());
            ViewBag.bName = _options.BucketName;
            return View(markedPhotos);
        }

        [HttpGet("{action}/{email?}/{userId?}")]
        public IActionResult AdminCreate(string email = null, int userId = 0)
        {
            var input = new AdminInputModel();
            if (email != null)
            {
                input.Email = email;
                input.UserId = userId;
            }
            else
            {
                input.UserId = _adminService.AddAdmin();
            }

            var user = _userManager.FindByNameAsync(User.Identity.Name).Result;
            input.SetUps = _adminService.GetSetUpsOfAdmin(user.Id)
                .ToList();
            return View(input);
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> Create(AdminInputModel input)
        {
            var link = HttpContext.Request.IsHttps ? "https://" : "http://";
            link += HttpContext.Request.Host.Value;
            var testUser = await _userManager.FindByEmailAsync(input.Email);
            if (testUser != null)
            {
                return Redirect(link + "/Admin/AdminCreate/" + input.Email + "/" + input.UserId);
            }

            var admin = _adminService.GetAdmin(input.UserId);
            var user = new UserAccount
            {
                UserName = input.Email, Email = input.Email
            };

            admin.UserAccount = user;
            user.User = admin;
            var result = await _userManager.CreateAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);

            admin = _adminService.GetAdmin(userId);
            admin.UserAccountId = userId;
            _adminService.UpdateAdmin(admin);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = link + "/admin/AdminConfirm/" + userId + "/" + code;
            await _emailSender.SendEmailAsync(input.Email, "Admin",
                $"{input.Email},{callbackUrl}");


            return Redirect(link + "/Admin/AdminGoodJob");
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> Confirm(AdminInputConfirmModel input)
        {
            var link = HttpContext.Request.IsHttps ? "https://" : "http://";
            link += HttpContext.Request.Host.Value;

            var user = await _userManager.FindByIdAsync(input.UserId);
            await _userManager.AddPasswordAsync(user, input.Password);

            await _userManager.AddToRoleAsync(user, "Admin");

            await _signInManager.SignInAsync(user, false);

            return Redirect(link + "/setups");
        }

        [HttpGet("{action}/{userId}/{code}")]
        public async Task<IActionResult> AdminConfirm(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);

            var input = new AdminInputConfirmModel()
            {
                UserId = userId
            };
            return View(input);
        }

        [HttpGet("{action}")]
        public IActionResult AdminGoodJob()
        {
            ViewBag.image = "https://storage.googleapis.com/make_vrt_great_again/" + _storage.GetRandomGif("admin");

            return View();
        }
    }
}