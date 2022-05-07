using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
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
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly UserManager<UserAccount> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly ITeacherService _teacherService;
        private readonly ISetupService _setupService;

        public ExternalLoginModel(SignInManager<UserAccount> signInManager, UserManager<UserAccount> userManager,
            IEmailSender emailSender, ILogger<ExternalLoginModel> logger, ITeacherService teacherService,
            ISetupService setupService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _emailSender = emailSender;
            _logger = logger;
            _teacherService = teacherService;
            _setupService = setupService;
        }

        [BindProperty] public InputModel Input { get; set; }

        public string LoginProvider { get; set; }
        public string SetupCode { get; set; }

        public string ReturnUrl { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required] [EmailAddress] public string Email { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, [FromQuery(Name = "setupCode")] string setupCode = null,
            string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new {returnUrl, setupCode});
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null,
            [FromQuery(Name = "setupCode")] string setupCode = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            SetupCode = setupCode;
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl, SetupCode = setupCode});
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl, SetupCode = setupCode});
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey,
                isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                    info.LoginProvider);
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                LoginProvider = info.LoginProvider;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }

                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(
            [FromQuery(Name = "setupCode")] string setupCode = null, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
            }

            if (ModelState.IsValid)
            {
                var setup = _setupService.ReadSetupByCode(setupCode);
                Teacher teacher = _teacherService.CreateTeacher(setup.SetUpId);


                var user = new UserAccount {UserName = Input.Email + "##" + setupCode, Email = Input.Email};
                teacher.UserAccount = user;
                user.User = teacher;
                var result = await _userManager.CreateAsync(user);
                string userId = await _userManager.GetUserIdAsync(user);
                teacher = _teacherService.GetTeacher(userId);
                teacher.UserAccountId = userId;
                _teacherService.UpdateTeacher(teacher);
                await _userManager.AddToRoleAsync(user, "Teacher");
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        /*if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }*/

                        await _signInManager.SignInAsync(user, isPersistent: false);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = user.Id, code = code },
                            protocol: Request.Scheme);
                        await _emailSender.SendEmailAsync(Input.Email, "ConfirmEmail",
                            $"{user.UserName},{user.UserName.Split("##")[0]},{setup.Name},{callbackUrl}");

                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            LoginProvider = info.LoginProvider;
            ReturnUrl = returnUrl;
            return Page();
        }
    }
}