using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyNet.Identity.UI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    //[ExtAbpIdentityDefaultUI(typeof(LoginModel<>))]
    public class LoginModel : PageModel
    {
        //private readonly IExtAbpSignInManager _signInManager;
        //private readonly ILogger<LoginModel> _logger;
        //private readonly ExtAbpIdentityDefaultUIOptions _options;

        //public LoginModel(IExtAbpSignInManager signInManager, IOptions<ExtAbpIdentityDefaultUIOptions> options, ILogger<LoginModel> logger)
        //{
        //    _options = options.Value;
        //    _signInManager = signInManager;
        //    _logger = logger;
        //}

        [BindProperty]
        public InputModel Input { get; set; }

        public Task OnGetAsync(string returnUrl = null)
        {
            return Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //returnUrl ??= _options.LoginSucceededRedirectPath;

            //if (ModelState.IsValid)
            //{
            //    // This doesn't count login failures towards account lockout
            //    // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //    var result = await _signInManager.PasswordSignInAsync(Input.LoginName, Input.Password, Input.RememberMe, lockoutOnFailure: false);
            //    if (result.Succeeded)
            //    {
            //        _logger.LogInformation("User logged in.");

            //        return Redirect(returnUrl);
            //    }
            //    if (result.RequiresTwoFactor)
            //    {
            //        return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
            //    }
            //    if (result.IsLockedOut)
            //    {
            //        _logger.LogWarning("User account locked out.");
            //        return RedirectToPage("./Lockout");
            //    }
            //    else
            //    {
            //        ModelState.AddModelError(string.Empty, "帐号或密码错误, 请重新输入");
            //        return Page();
            //    }
            //}

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