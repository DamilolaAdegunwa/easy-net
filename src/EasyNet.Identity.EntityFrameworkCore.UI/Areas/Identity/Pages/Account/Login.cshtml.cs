using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using EasyNet.Identity.EntityFrameworkCore.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyNet.Identity.EntityFrameworkCore.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IEasyNetGeneralSignInManager _signInManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly EasyNetIdentityDefaultUIOptions _options;

        public LoginModel(IEasyNetGeneralSignInManager signInManager, IOptions<EasyNetIdentityDefaultUIOptions> options, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
            _options = options.Value;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet(string returnUrl = null)
        {
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (returnUrl == null)
            {
                returnUrl = _options.RedirectPathAfterLogin;
            }

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.LoginName, Input.Password, Input.RememberMe, lockoutOnFailure: _options.LockoutOnFailure);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");

                    return Redirect(returnUrl);
                }
                if (result.RequiresTwoFactor)

                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    ModelState.AddModelError(string.Empty, "账号已经被锁定.");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "帐号或密码错误, 请重新输入.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        public class InputModel
        {
            [Required(ErrorMessage = "请您输入邮件/用户名")]
            [Display(Name = "邮件/用户名")]
            public string LoginName { get; set; }

            [Required(ErrorMessage = "请您输入密码")]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; }

            [Display(Name = "记住我")]
            public bool RememberMe { get; set; }
        }
    }
}