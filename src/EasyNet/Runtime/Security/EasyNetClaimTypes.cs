using System.Security.Claims;

namespace EasyNet.Runtime.Security
{
	/// <summary>
	/// Used to get EasyNet specific claim type names.
	/// </summary>
	public class EasyNetClaimTypes
	{
		/// <summary>
		/// User id.
		/// Default: <see cref="ClaimTypes.Name"/>
		/// </summary>
		public static string UserName { get; set; } = ClaimTypes.Name;

		/// <summary>
		/// UserName.
		/// Default: <see cref="ClaimTypes.NameIdentifier"/>
		/// </summary>
		public static string UserId { get; set; } = ClaimTypes.NameIdentifier;

		/// <summary>
		/// Tenant id.
		/// Default: http://www.easynet.com/identity/claims/tenantId
		/// </summary>
		public static string TenantId { get; set; } = "http://www.easynet.com/identity/claims/tenantId";

		/// <summary>
		/// User role.
		/// Default: <see cref="ClaimTypes.Role"/>
		/// </summary>
		public static string Role { get; set; } = ClaimTypes.Role;
	}
}
