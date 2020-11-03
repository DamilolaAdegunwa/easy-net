using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EasyNet.Identity.EntityFrameworkCore.UI.Areas.Identity.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        [BindProperty]
        public InputModel Input { get; set; }

        public void OnGet()
        {

        }

        public class InputModel
        {
            [Required]
            public string Email { get; set; }
        }
    }
}