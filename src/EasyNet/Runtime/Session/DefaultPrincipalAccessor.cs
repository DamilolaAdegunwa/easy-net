using System.Security.Claims;
using System.Threading;

namespace EasyNet.Runtime.Session
{
	public class DefaultPrincipalAccessor : IPrincipalAccessor
	{
		public virtual ClaimsPrincipal Principal => Thread.CurrentPrincipal as ClaimsPrincipal;
	}
}
