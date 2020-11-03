using System;
using EasyNet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
	public class EasyNetRole : EasyNetRole<int>
	{
	}

    public class EasyNetRole<TPrimaryKey> : IdentityRole<TPrimaryKey>, IEntity<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
    }
}
