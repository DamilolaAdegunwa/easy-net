using System;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
	public class EasyNetUserToken : EasyNetUserToken<int>
	{
	}

    public class EasyNetUserToken<TUserKey> : IdentityUserToken<TUserKey>
       where TUserKey : IEquatable<TUserKey>
    {
    }
}
