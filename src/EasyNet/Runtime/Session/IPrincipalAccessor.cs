using System.Security.Claims;

namespace EasyNet.Runtime.Session
{
	public interface IPrincipalAccessor
	{
		ClaimsPrincipal Principal { get; }
	}
}
