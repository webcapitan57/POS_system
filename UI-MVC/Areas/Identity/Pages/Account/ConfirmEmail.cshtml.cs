using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;

namespace UI_MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<UserAccount> _userManager;
        private readonly ITeacherService _teacherService;
        private readonly SignInManager<UserAccount> _signInManager;
        
        public ConfirmEmailModel(UserManager<UserAccount> userManager, ITeacherService teacherService, SignInManager<UserAccount> signInManager)
        {
            _userManager = userManager;
            _teacherService = teacherService;
            _signInManager = signInManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
           
            result= await _userManager.AddToRoleAsync(user, "Teacher");
            var teacher = _teacherService.GetTeacher(user.Id);
            teacher.IsGuest = false;
            _teacherService.UpdateTeacher(teacher);
            await _signInManager.SignInAsync(user,false);
            
            return Page();
        }
    }
}
