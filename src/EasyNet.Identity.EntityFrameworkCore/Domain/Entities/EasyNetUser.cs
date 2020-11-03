using System;
using System.Reflection;
using System.Security.Cryptography;
using EasyNet.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace EasyNet.Identity.EntityFrameworkCore.Domain.Entities
{
	public class EasyNetUser : EasyNetUser<int>
	{
	}

    public class EasyNetUser<TPrimaryKey> : IdentityUser<TPrimaryKey>, IEntity<TPrimaryKey>
        where TPrimaryKey : IEquatable<TPrimaryKey>
    {
        /// <summary>
        /// Normalized UserName and Email in order to optimize database queries.
        /// </summary>
        public virtual void SetNormalizedNames()
        {
            if (!string.IsNullOrEmpty(UserName))
            {
                NormalizedUserName = UserName.ToUpperInvariant();
            }

            if (!string.IsNullOrEmpty(Email))
            {
                NormalizedEmail = Email.ToUpperInvariant();
            }
        }

        public virtual void SetNewSecurityStamp()
        {
            byte[] numArray = new byte[20];
            var rnd = RandomNumberGenerator.Create();
            rnd.GetBytes(numArray);

            var assembly = typeof(UserManager<>).Assembly;
            var type = assembly.GetType("Microsoft.AspNetCore.Identity.Base32");
            SecurityStamp = type.InvokeMember(
                "ToBase32",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public,
                null,
                null,
                new object[] { numArray }).ToString();
        }
    }
}
