using System;
using System.Linq;
using EasyNet.EntityFrameworkCore;
using EasyNet.Identity.EntityFrameworkCore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace EasyNet.Identity.EntityFrameworkCore.DbContext
{
	public class EasyNetIdentityUserContext : EasyNetIdentityUserContext<EasyNetUser<int>, EasyNetUserClaim<int>, EasyNetUserLogin<int>, EasyNetUserToken<int>, int>
	{
		public EasyNetIdentityUserContext(DbContextOptions options)
			: base(options)
		{
		}
	}

	public class EasyNetIdentityUserContext<TUser> : EasyNetIdentityUserContext<TUser, EasyNetUserClaim<int>, EasyNetUserLogin<int>, EasyNetUserToken<int>, int>
	 where TUser : EasyNetUser<int>
	{
		public EasyNetIdentityUserContext(DbContextOptions options)
			: base(options)
		{
		}
	}

	public class EasyNetIdentityUserContext<TUser, TPrimaryKey> : EasyNetIdentityUserContext<TUser, EasyNetUserClaim<TPrimaryKey>, EasyNetUserLogin<TPrimaryKey>, EasyNetUserToken<TPrimaryKey>, TPrimaryKey>
		where TUser : EasyNetUser<TPrimaryKey>
		where TPrimaryKey : IEquatable<TPrimaryKey>
	{
		public EasyNetIdentityUserContext(DbContextOptions options)
			: base(options)
		{
		}
	}

	public class EasyNetIdentityUserContext<TUser, TUserClaim, TUserLogin, TUserToken, TPrimaryKey> : EasyNetDbContext
		where TUser : EasyNetUser<TPrimaryKey>
		where TUserClaim : EasyNetUserClaim<TPrimaryKey>
		where TUserLogin : EasyNetUserLogin<TPrimaryKey>
		where TUserToken : EasyNetUserToken<TPrimaryKey>
		where TPrimaryKey : IEquatable<TPrimaryKey>
	{
		private const string CanOnlyProtectStrings = "[ProtectedPersonalData] only works strings by default.";

		public EasyNetIdentityUserContext(DbContextOptions options)
			: base(options)
		{
		}

		public virtual DbSet<TUser> Users { get; set; }

		public virtual DbSet<TUserClaim> UserClaims { get; set; }

		public virtual DbSet<TUserLogin> UserLogins { get; set; }

		public virtual DbSet<TUserToken> UserTokens { get; set; }

		private StoreOptions GetStoreOptions() => this.GetService<IDbContextOptions>()
							.Extensions.OfType<CoreOptionsExtension>()
							.FirstOrDefault()?.ApplicationServiceProvider
							?.GetService<IOptions<IdentityOptions>>()
							?.Value?.Stores;

		private class PersonalDataConverter : ValueConverter<string, string>
		{
			public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
			{ }
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			var storeOptions = GetStoreOptions();
			var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
			var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
			PersonalDataConverter converter = null;

			builder.Entity<TUser>(b =>
			{
				b.HasKey(u => u.Id);
				b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
				b.HasIndex(u => u.NormalizedEmail).HasName("EmailIndex");
				b.ToTable("Users");
				b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();

				b.Property(u => u.UserName).HasMaxLength(256);
				b.Property(u => u.NormalizedUserName).HasMaxLength(256);
				b.Property(u => u.Email).HasMaxLength(256);
				b.Property(u => u.NormalizedEmail).HasMaxLength(256);

				if (encryptPersonalData)
				{
					converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
					var personalDataProps = typeof(TUser).GetProperties().Where(
									prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
					foreach (var p in personalDataProps)
					{
						if (p.PropertyType != typeof(string))
						{
							throw new InvalidOperationException(CanOnlyProtectStrings);
						}
						b.Property(typeof(string), p.Name).HasConversion(converter);
					}
				}

				b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
				b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
				b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
			});

			builder.Entity<TUserClaim>(b =>
			{
				b.HasKey(uc => uc.Id);
				b.ToTable("UserClaims");
			});

			builder.Entity<TUserLogin>(b =>
			{
				b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

				if (maxKeyLength > 0)
				{
					b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
					b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
				}

				b.ToTable("UserLogins");
			});

			builder.Entity<TUserToken>(b =>
			{
				b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

				if (maxKeyLength > 0)
				{
					b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
					b.Property(t => t.Name).HasMaxLength(maxKeyLength);
				}

				if (encryptPersonalData)
				{
					var tokenProps = typeof(TUserToken).GetProperties().Where(
									prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
					foreach (var p in tokenProps)
					{
						if (p.PropertyType != typeof(string))
						{
							throw new InvalidOperationException(CanOnlyProtectStrings);
						}
						b.Property(typeof(string), p.Name).HasConversion(converter);
					}
				}

				b.ToTable("UserTokens");
			});
		}
	}
}
