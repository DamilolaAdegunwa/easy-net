using System;
using EasyNet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
    public class EasyNetUserToken : EasyNetUserToken<int>
    {
    }

    public class EasyNetUserToken<TUserKey> : IdentityUserToken<TUserKey>, IEntity<int>
       where TUserKey : IEquatable<TUserKey>
    {
        public int Id { get; set; }
    }
}
