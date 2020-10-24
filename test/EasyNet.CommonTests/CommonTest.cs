using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EasyNet.CommonTests
{
	public class CommonTest
	{
		public static IWebHostEnvironment GetHostingEnvironment()
		{
			var environment = new Mock<IWebHostEnvironment>();
			environment
				.Setup(e => e.ApplicationName)
				.Returns(typeof(CommonTest).GetTypeInfo().Assembly.GetName().Name);

			return environment.Object;
		}

		public static ActionExecutingContext CreateControllerActionExecutingContext(IFilterMetadata filter)
		{
			return new ActionExecutingContext(
				CreateControllerActionContext(),
				new[] { filter },
				new Dictionary<string, object>(),
				controller: new object());
		}

		public static ActionExecutingContext CreateActionExecutingContext(IFilterMetadata filter)
		{
			return new ActionExecutingContext(
				CreateActionContext(),
				new[] { filter },
				new Dictionary<string, object>(),
				controller: new object());
		}

		public static ActionExecutedContext CreateActionExecutedContext(ActionExecutingContext context)
		{
			return new ActionExecutedContext(context, context.Filters, context.Controller)
			{
				Result = context.Result
			};
		}

		private static ActionContext CreateActionContext()
		{
			return new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
		}

		private static ActionContext CreateControllerActionContext()
		{
			return new ActionContext(new DefaultHttpContext(), new RouteData(), new ControllerActionDescriptor());
		}
	}
}
