using System;
using System.Linq;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using EasyNet.Runtime.Initialization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace EasyNet.Identity.EntityFrameworkCore.Initialization
{
    public class AdminInitializationJob<TUser, TPrimaryKey> : IEasyNetInitializationJob
        where TUser : EasyNetUser<TPrimaryKey>, new()
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        private readonly UserManager<TUser> _userManager;
        private readonly DefaultAdminUserOptions _defaultAdminUserOptions;

        public AdminInitializationJob(UserManager<TUser> userManager, IOptions<DefaultAdminUserOptions> options)
        {
            _userManager = userManager;
            _defaultAdminUserOptions = options.Value;
        }

        public void Start()
        {
            var adminUser = _userManager.Users.FirstOrDefault(p => p.IsAdmin);

            if (adminUser == null)
            {
                adminUser = new TUser
                {
                    UserName = _defaultAdminUserOptions.UserName,
                    Email = !string.IsNullOrEmpty(_defaultAdminUserOptions.Email) ? _defaultAdminUserOptions.Email : null,
                    IsAdmin = true
                };

                var identityResult = _userManager.CreateAsync(adminUser, _defaultAdminUserOptions.Password).Result;

                if (!identityResult.Succeeded)
                {
                    throw new Exception("Add default admin user failed. Error:" + string.Join(",", identityResult.Errors.Select(p => p.Description)));
                }
            }
        }
    }
}
