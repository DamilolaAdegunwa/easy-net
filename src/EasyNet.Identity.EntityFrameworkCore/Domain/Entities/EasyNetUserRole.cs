using System;
using EasyNet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
    public class EasyNetUserRole : EasyNetUserRole<int>
    {
    }

    public class EasyNetUserRole<TUserRoleKey> : IdentityUserRole<TUserRoleKey>, IEntity<int>
        where TUserRoleKey : IEquatable<TUserRoleKey>
    {
        public int Id { get; set; }
    }
}
