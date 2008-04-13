using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class ServiceAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceAttribute"/> class.
		/// </summary>
		public ServiceAttribute()
		{
			Order = 0;
		}

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter filterContext.</param>
		public override void OnActionExecuting(FilterExecutingContext filterContext)
		{
			string type = filterContext.HttpContext.Request.QueryString["type"];
			ResponseType responseType = ResponseType.Html;

			// check to see if we should try to parse it to an enum
			if (!String.IsNullOrEmpty(type))
				responseType = ManagedFusion.Utility.ParseEnum<ResponseType>(type);

			// if the response type is still the default HTML check the Accept header
			// if the requestion is an XMLHttpRequest
			if (responseType == ResponseType.Html 
				&& filterContext.HttpContext.Request.AcceptTypes != null
				&& String.Equals(filterContext.HttpContext.Request.Headers["x-requested-with"], "XMLHttpRequest", StringComparison.OrdinalIgnoreCase))
			{
				foreach (string accept in filterContext.HttpContext.Request.AcceptTypes)
				{
					switch (accept.ToLower())
					{
						case "application/json": 
						case "application/x-json": responseType = ResponseType.Json; break;

						case "application/javascript":
						case "application/x-javascript":
						case "text/javascript": responseType = ResponseType.JS; break;
						
						case "application/xml":
						case "text/xml": responseType = ResponseType.Xml; break;
					}

					if (responseType != ResponseType.Html)
						break;
				}
			}

			// set the view engine to a service if anything other than HTML
			if (responseType != ResponseType.Html && filterContext.Controller is Controller)
				((Controller)filterContext.Controller).ViewEngine = new ServiceViewEngine(responseType);

			if (filterContext.RouteData.Values.ContainsKey("responseType"))
				filterContext.RouteData.Values.Remove("responseType");

			// set the value in the route data so it can be used in the methods
			filterContext.RouteData.Values.Add("responseType", responseType);
		}
	}
}
