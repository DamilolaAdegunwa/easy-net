using System;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
	public class EasyNetUserRole : EasyNetUserRole<int>
	{
	}

    public class EasyNetUserRole<TUserRoleKey> : IdentityUserRole<TUserRoleKey>
        where TUserRoleKey : IEquatable<TUserRoleKey>
    {
    }
}
