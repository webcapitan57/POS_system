using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITeacherService _teacherService;
        private readonly ISetupService _setupService;

        public RegisterModel(SignInManager<UserAccount> signInManager, UserManager<UserAccount> userManager, ILogger<RegisterModel> logger, IEmailSender emailSender, ITeacherService teacherService, ISetupService setupService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _teacherService = teacherService;
            _setupService = setupService;
        }


        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }
        public string SetUpCode { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }
            
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(30, ErrorMessage = "Het wachtwoord moet minstens {2} en maximum {1} tekens lang zijn.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Het wachtwoord en het bevestigingswachtwoord komen niet overeen.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync([FromQuery(Name = "setupCode")]string setupCode,string returnUrl = null)
        {
            SetUpCode = setupCode;
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        // Use http in staging environment, https in production environment
        public async Task<IActionResult> OnPostAsync([FromQuery(Name = "setupCode")]string setUpCode,string returnUrl = null)
        {
            //TODO hardcoded setUpId
            
            returnUrl = returnUrl ?? Url.Content("~/");
            SetUpCode = setUpCode;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var userAccount = _userManager.FindByNameAsync(Input.Username + "_" + setUpCode ).Result;
            var setup = _setupService.ReadSetupByCode(setUpCode);
            if (userAccount == null)
            {
                if (ModelState.IsValid)
                {

                    Teacher teacher = _teacherService.CreateTeacher(setup.SetUpId);
                    var user = new UserAccount
                    {
                        UserName = Input.Username + "##" + setUpCode, Email = Input.Email
                    };
                    teacher.UserAccount = user;
                    user.User = teacher;
                    var result = await _userManager.CreateAsync(user, Input.Password);
                    string userId = await _userManager.GetUserIdAsync(user);
                    teacher = _teacherService.GetTeacher(userId);
                    teacher.UserAccountId = userId;
                    _teacherService.UpdateTeacher(teacher);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code },
                        protocol: Request.Scheme);
                    await _emailSender.SendEmailAsync(Input.Email, "ConfirmEmail",
                        $"{user.UserName},{user.UserName.Split("##")[0]},{setup.Name},{callbackUrl}");

                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("RegisterConfirmation", new {email = Input.Email});
                        }
                        else
                        {
                            await _signInManager.SignInAsync(user, isPersistent: false);
                            return LocalRedirect(returnUrl);
                        }
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }


            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
