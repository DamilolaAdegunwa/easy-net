using System;
using System.Collections.Generic;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EasyNet.Identity.EntityFrameworkCore.Domain
{
    public class EasyNetUserManager<TUser, TPrimaryKey> : UserManager<TUser>
        where TUser : EasyNetUser<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        public EasyNetUserManager(
            IUserStore<TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger) :
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
