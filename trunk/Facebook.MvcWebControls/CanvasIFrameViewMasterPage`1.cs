using System;
using System.Globalization;
using System.Web;

namespace Facebook.MvcWebControls
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TViewData">The type of the view data.</typeparam>
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CanvasIFrameViewMasterPage<TViewData> : CanvasIFrameViewMasterPage
	{
		/// <summary>
		/// Gets the view data.
		/// </summary>
		/// <value>The view data.</value>
		public new TViewData ViewData
		{
			get
			{
				CanvasIFrameViewPage<TViewData> viewPage = Page as CanvasIFrameViewPage<TViewData>;
				if (viewPage == null)
				{
					throw new InvalidOperationException(String.Format(
						CultureInfo.CurrentUICulture, 
						"A ViewMasterPage<TViewData> can only be used with content pages that derive from ViewPage<TViewData>."
						));
				}
				return viewPage.ViewData;
			}
		}
	}
}
