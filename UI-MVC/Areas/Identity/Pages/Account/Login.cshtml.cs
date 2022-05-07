using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly SignInManager<UserAccount> _signInManager;
        private readonly ISetupService _setupService;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(UserManager<UserAccount> userManager, SignInManager<UserAccount> signInManager,
            ISetupService setupService, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _setupService = setupService;
            _logger = logger;
        }

        [BindProperty] public InputModel Input { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public string ReturnUrl { get; set; }
        public string SetUpCode { get; set; }

        [TempData] public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required] public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync([FromQuery(Name = "setupCode")] string setUpCode, string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            var setup = _setupService.ReadSetupByCode(setUpCode);
            if (setup != null && setup.Archived)
            {
                SetUpCode = "ARCHIVED";
            }
            else
            {
                SetUpCode = setUpCode;
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync([FromQuery(Name = "setupCode")] string setUpCode,
            string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            SetUpCode = setUpCode;
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                UserAccount user = null;
                if (setUpCode != null)
                {
                    user = await _userManager.FindByNameAsync(Input.Username + "##" + setUpCode);
                }
                else
                {
                    user = await _userManager.FindByEmailAsync(Input.Username);
                }

                if (user == null) return Page();
                var result = await _signInManager.PasswordSignInAsync(user.UserName, Input.Password,
                    Input.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    if (_userManager.IsInRoleAsync(user, "Admin").Result)
                    {
                        return Redirect("/setUps");
                    }

                    if (_userManager.IsInRoleAsync(user, "Teacher").Result)
                    {
                        return Redirect("/group/index");
                    }
                }

                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa",
                        new {ReturnUrl = returnUrl, RememberMe = Input.RememberMe});
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}