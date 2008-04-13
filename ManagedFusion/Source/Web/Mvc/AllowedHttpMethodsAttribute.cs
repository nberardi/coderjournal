﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	public class AllowedHttpMethodsAttribute : ActionFilterAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="AllowedHttpMethodsAttribute"/> class.
		/// </summary>
		public AllowedHttpMethodsAttribute()
			: this("OPTIONS", "GET", "HEAD", "POST", "PUT", "DELETE", "TRACE", "CONNECT") { }

		/// <summary>
		/// Initializes a new instance of the <see cref="AllowedHttpMethodsAttribute"/> class.
		/// </summary>
		/// <param name="httpMethods">The HTTP methods.</param>
		public AllowedHttpMethodsAttribute(params string[] httpMethods)
		{
			for (int i = 0; i < httpMethods.Length; i++)
				httpMethods[i] = httpMethods[i].ToUpper();

			SupportedHttpMethods = new ReadOnlyCollection<string>(httpMethods);
		}

		/// <summary>
		/// Gets or sets the supported HTTP methods.
		/// </summary>
		/// <value>The supported HTTP methods.</value>
		public IList<string> SupportedHttpMethods { get; private set; }

		/// <summary>
		/// Called when [action executing].
		/// </summary>
		/// <param name="filterContext">The filter filterContext.</param>
		public override void OnActionExecuting(FilterExecutingContext filterContext)
		{
			if (!SupportedHttpMethods.Contains(filterContext.HttpContext.Request.HttpMethod.ToUpper()))
			{
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.MethodNotAllowed;
				filterContext.HttpContext.Response.StatusDescription = "Method Not Allowed";
				filterContext.HttpContext.Response.AppendHeader("Allow", String.Join(", ", this.SupportedHttpMethods.ToArray()));
				filterContext.HttpContext.Response.ContentType = "text/html";
				filterContext.HttpContext.Response.Write("<html><head><title>Method Not Allowed</title></head><h1>Method Not Allowed</h1><p>Only " + String.Join(", ", this.SupportedHttpMethods.ToArray()) + " is allowed.</p></html>");
				filterContext.HttpContext.Response.Flush();
				filterContext.HttpContext.ApplicationInstance.CompleteRequest();
				filterContext.Cancel = true;
			}
		}
	}
}
