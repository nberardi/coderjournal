using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

using ManagedFusion.Serialization;

namespace ManagedFusion.Web.Mvc
{
	/// <summary>
	/// 
	/// </summary>
	internal class ServiceViewEngine : IViewEngine
	{
		/// <summary>
		/// Builds the response.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns></returns>
		internal static Dictionary<string, object> BuildResponse(object serializableObject, Dictionary<string, object> serializedContent)
		{
			// create body of the response
			Dictionary<string, object> response = new Dictionary<string, object>();
			response.Add("timestamp", DateTime.UtcNow);

			// check for regular collection
			if (serializableObject is ICollection)
			{
				response.Add("count", ((ICollection)serializableObject).Count);

				if (serializedContent.Count > 1)
					response.Add("collection", serializedContent);
				else
					foreach (var value in serializedContent)
						response.Add(value.Key, value.Value);
			}
			else if (serializedContent.Count > 1)
				response.Add("object", serializedContent);
			else
				foreach (var value in serializedContent)
					response.Add(value.Key, value.Value);

			return response;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceViewEngine"/> class.
		/// </summary>
		/// <param name="responseFormat">The response format.</param>
		public ServiceViewEngine(ResponseType responseFormat)
		{
			ResponseFormat = responseFormat;
		}

		/// <summary>
		/// Gets or sets the response format.
		/// </summary>
		/// <value>The response format.</value>
		public ResponseType ResponseFormat { get; private set; }

		#region IViewEngine Members

		/// <summary>
		/// Renders the view.
		/// </summary>
		/// <param name="viewContext">The view filterContext.</param>
		public void RenderView(ViewContext viewContext)
		{
			HttpResponseBase response = viewContext.HttpContext.Response;
			HttpRequestBase request = viewContext.HttpContext.Request;

			response.ContentEncoding = System.Text.Encoding.UTF8;
			response.Cache.SetCacheability(HttpCacheability.Private);
			response.Cache.AppendCacheExtension("no-cache, no-cache=\"Set-Cookie\", proxy-revalidate");
			response.ClearContent();

			switch (ResponseFormat)
			{
				case ResponseType.Xml:
					goto default;

				case ResponseType.Json:
					response.AppendHeader("X-Robots-Tag", "noindex, follow, noarchive, nosnippet");
					response.ContentType = "application/json";
					response.Write(viewContext.ViewData.Serialize(new JsonSerializer(viewContext.ViewData)));
					break;

				case ResponseType.JS:
					response.AppendHeader("X-Robots-Tag", "noindex, follow, noarchive, nosnippet");
					response.ContentType = "text/javascript";

					string callback = viewContext.HttpContext.Request.QueryString["callback"];

					if (String.IsNullOrEmpty(callback))
						callback = "callback";

					response.Write(callback);
					response.Write("(");
					response.Write(viewContext.ViewData.Serialize(new JsonSerializer(viewContext.ViewData)));
					response.Write(");");
					break;

				default:
					response.AppendHeader("X-Robots-Tag", "noindex, follow, noarchive, nosnippet");
					response.ContentType = "text/xml";
					response.Write(viewContext.ViewData.Serialize(new XmlSerializer(viewContext.ViewData)));
					break;
			}

			// complete request
			viewContext.HttpContext.ApplicationInstance.CompleteRequest();
		}

		#endregion
	}
}
