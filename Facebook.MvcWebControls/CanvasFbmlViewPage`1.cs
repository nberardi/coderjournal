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
	public class CanvasFbmlViewPage<TViewData> : CanvasFbmlViewPage
	{
		private TViewData _viewData;

		/// <summary>
		/// Gets the view data.
		/// </summary>
		/// <value>The view data.</value>
		public new TViewData ViewData
		{
			get { return _viewData; }
		}

		/// <summary>
		/// Sets the view data.
		/// </summary>
		/// <param name="viewData">The view data.</param>
		protected override void SetViewData(object viewData)
		{
			if (viewData != null && !(viewData is TViewData))
			{
				throw new ArgumentException(
					String.Format(
						CultureInfo.CurrentUICulture,
						"The view data passed into the page is of type '{0}' but this page requires view data of type '{1}'.",
						viewData.GetType(),
						typeof(TViewData)),
					"viewData");
			}

			_viewData = (TViewData)viewData;
			base.SetViewData(viewData);
		}
	}
}
