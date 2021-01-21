using System;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EasyNet.Identity.EntityFrameworkCore.Domain
{
    public class EasyNetRoleStore<TRole, TDbContext, TPrimaryKey, TUserRole, TRoleClaim> : RoleStore<TRole, TDbContext, TPrimaryKey, TUserRole, TRoleClaim>
        where TRole : EasyNetRole<TPrimaryKey>
        where TDbContext : Microsoft.EntityFrameworkCore.DbContext
        where TPrimaryKey : IEquatable<TPrimaryKey>
        where TUserRole : EasyNetUserRole<TPrimaryKey>, new()
        where TRoleClaim : EasyNetRoleClaim<TPrimaryKey>, new()
    {
        public EasyNetRoleStore(TDbContext context, IdentityErrorDescriber describer = null) : base(context, describer)
        {
        }
    }
}
