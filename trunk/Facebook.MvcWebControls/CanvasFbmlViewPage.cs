using System;
using System.Web;
using System.Web.Mvc;
using Facebook.Components;

namespace Facebook.MvcWebControls
{
	/// <summary>
	/// 
	/// </summary>
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(System.Security.Permissions.SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class CanvasFbmlViewPage : ViewPage
    {
        FacebookService _fbService = new FacebookService();
        private string _api = null;
        private string _secret = null;
        private bool _autoAdd = true;

		/// <summary>
		/// Gets or sets the API.
		/// </summary>
		/// <value>The API.</value>
        public string Api
        {
            get { return _api; }
            set { _api = value; }
        }
		/// <summary>
		/// Gets or sets the secret.
		/// </summary>
		/// <value>The secret.</value>
        public string Secret
        {
            get { return _secret; }
            set { _secret = value; }
        }
		/// <summary>
		/// Gets or sets a value indicating whether [auto add].
		/// </summary>
		/// <value>
		/// 	<see langword="true"/> if [auto add]; otherwise, <see langword="false"/>.
		/// </value>
        public bool AutoAdd
        {
            get { return _autoAdd; }
            set { _autoAdd = value; }
        }
		/// <summary>
		/// Gets the FB service.
		/// </summary>
		/// <value>The FB service.</value>
        public FacebookService FBService
        {
            get { return _fbService; }
        }

		/// <summary>
		/// Handles the Load event of the Page control.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // ApplicationKey and Secret are acquired when you sign up for 
            _fbService.ApplicationKey = _api;
            _fbService.Secret = _secret;

            BasePageHelper.LoadFBMLPage(_fbService, _autoAdd, Request, Response);
        }
    }
}
