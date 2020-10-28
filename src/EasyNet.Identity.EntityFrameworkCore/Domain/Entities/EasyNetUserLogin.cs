using System;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
	public class EasyNetUserLogin : EasyNetUserLogin<int>
	{
	}

    public class EasyNetUserLogin<TUserKey> : IdentityUserLogin<TUserKey>
       where TUserKey : IEquatable<TUserKey>
    {
    }
}
