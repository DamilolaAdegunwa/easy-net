using System;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EasyNet.Identity.EntityFrameworkCore.Domain
{
    public class EasyNetUserStore<TUser, TRole, TDbContext, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim> : UserStore<TUser, TRole, TDbContext, TPrimaryKey, TUserClaim, TUserRole, TUserLogin, TUserToken, TRoleClaim>
        where TUser : EasyNetUser<TPrimaryKey>
        where TRole : EasyNetRole<TPrimaryKey>
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        where TPrimaryKey : IEquatable<TPrimaryKey>
        where TUserClaim : EasyNetUserClaim<TPrimaryKey>, new()
        where TUserRole : EasyNetUserRole<TPrimaryKey>, new()
        where TUserLogin : EasyNetUserLogin<TPrimaryKey>, new()
        where TUserToken : EasyNetUserToken<TPrimaryKey>, new()
        where TRoleClaim : EasyNetRoleClaim<TPrimaryKey>, new()
    {
        public EasyNetUserStore(TDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
