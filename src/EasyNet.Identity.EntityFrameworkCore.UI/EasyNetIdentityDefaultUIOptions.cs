using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EasyNet.Identity.EntityFrameworkCore.UI
{
    public class EasyNetIdentityDefaultUiOptions
    {
        public string RedirectPathAfterLogin { get; set; }

        public bool LockoutOnFailure { get; set; } = true;
    }
}
