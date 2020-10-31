using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNet.Identity.EntityFrameworkCore.Sample.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EasyNet.Identity.EntityFrameworkCore.Sample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IdentityContext _identityContext;

        public IndexModel(IdentityContext identityContext)
        {
            _identityContext = identityContext;
        }

        public void OnGet()
        {

        }

        public async Task OnPostAsync()
        {
            await _identityContext.Database.MigrateAsync();
        }
    }
}
