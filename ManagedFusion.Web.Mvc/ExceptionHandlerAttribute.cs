/** 
 * Copyright (C) 2007-2008 Nicholas Berardi, Managed Fusion, LLC (nick@managedfusion.com)
 * 
 * <author>Nicholas Berardi</author>
 * <author_email>nick@managedfusion.com</author_email>
 * <company>Managed Fusion, LLC</company>
 * <license>Microsoft Public License (Ms-PL)</license>
 * <agreement>
 * This software, as defined above in <product />, is copyrighted by the <author /> and the <company /> 
 * and is licensed for use under <license />, all defined above.
 * 
 * This copyright notice may not be removed and if this <product /> or any parts of it are used any other
 * packaged software, attribution needs to be given to the author, <author />.  This can be in the form of a textual
 * message at program startup or in documentation (online or textual) provided with the packaged software.
 * </agreement>
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
	public class ExceptionHandlerAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionHandlerAttribute"/> class.
		/// </summary>
		/// <param name="action">The action.</param>
		/// <param name="controller">The controller.</param>
		/// <param name="exceptionTypes">The exception types.</param>
		public ExceptionHandlerAttribute(string action, string controller, params Type[] exceptionTypes)
		{
			Redirect = true;
			Action = action;
			Controller = controller;
			ExceptionTypes = exceptionTypes;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ExceptionHandlerAttribute"/> class.
		/// </summary>
		/// <param name="responseCode">The response code.</param>
		/// <param name="responseDescription">The response description.</param>
		/// <param name="exceptionTypes">The exception types.</param>
		public ExceptionHandlerAttribute(int responseCode, string responseDescription, params Type[] exceptionTypes)
		{
			Redirect = false;
			ResponseCode = responseCode;
			ResponseDescription = responseDescription;
			ExceptionTypes = exceptionTypes;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="ExceptionHandlerAttribute"/> is redirect.
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if redirect; otherwise, <see langword="false"/>.
		/// </value>
		public bool Redirect { get; private set; }

		/// <summary>
		/// Gets or sets the response code.
		/// </summary>
		/// <value>The response code.</value>
		public int ResponseCode { get; private set; }

		/// <summary>
		/// Gets or sets the response description.
		/// </summary>
		/// <value>The response description.</value>
		public string ResponseDescription { get; private set; }

		/// <summary>
		/// Gets or sets the action.
		/// </summary>
		/// <value>The action.</value>
		public string Action { get; private set; }

		/// <summary>
		/// Gets or sets the controller.
		/// </summary>
		/// <value>The controller.</value>
		public string Controller { get; private set; }

		/// <summary>
		/// Gets or sets the exception types.
		/// </summary>
		/// <value>The exception types.</value>
		public Type[] ExceptionTypes { get; private set; }

		/// <summary>
		/// Called when [action executed].
		/// </summary>
		/// <param name="filterContext">The filter context.</param>
		public override void OnActionExecuted(FilterExecutedContext filterContext)
		{
			Exception exception = filterContext.Exception;

			if (exception == null)
				return;

			foreach (Type exceptionType in ExceptionTypes)
			{
				if (exceptionType.IsAssignableFrom((exception.InnerException ?? exception).GetType()))
				{
					filterContext.ExceptionHandled = false;

					if (Redirect)
					{
						if (String.Equals("POST", filterContext.HttpContext.Request.HttpMethod, StringComparison.OrdinalIgnoreCase))
							filterContext.RedirectToAction(303, Action, Controller);
						else
							filterContext.RedirectToAction(307, Action, Controller);
					}
					else
					{
						filterContext.HttpContext.Response.Clear();

						filterContext.HttpContext.Response.StatusCode = ResponseCode;
						filterContext.HttpContext.Response.StatusDescription = ResponseDescription;

						filterContext.HttpContext.Response.ContentType = "text/html";
						filterContext.HttpContext.Response.Write("<html><head><title>" + ResponseDescription + "</title></head><body>");
						filterContext.HttpContext.Response.Write("<h2>" + ResponseDescription + "</h2>");
						filterContext.HttpContext.Response.Write("<p>" + (exception.InnerException ?? exception).Message + "</p>");
						filterContext.HttpContext.Response.Write("</body></html>");

						filterContext.HttpContext.Response.End();
					}
				}
			}
		}
	}
}
