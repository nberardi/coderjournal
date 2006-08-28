using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ManagedFusion.Modules;
using ManagedFusion.Syndication;

namespace CoderJournal.Modules.Journal
{
	[Module(
		"Journal",
		"Main content distribution for Coder Journal.",
		"{E6C67CAB-9B0F-4063-9D1B-698C92566EDD}",
		"Journal",
		TraversablePath = true,
		ConfigInFolder = true)]
	public class JournalModule : ModuleBase
	{
		protected override void OnLoadSyndication(LoadSyndicationEventArgs e)
		{
			string currentPageString = Context.Request.QueryString["p"];
			string pageSizeString = Context.Request.QueryString["s"];

			int currentPage = 0;
			int pageSize = 10;

			if (Int32.TryParse(currentPageString, out currentPage) == false)
				currentPage = 0;

			if (Int32.TryParse(pageSizeString, out pageSize) == false)
				pageSize = 10;

			// this statment checks to see if a sitemap request is being made and if one is
			// it returns all contents in the database
			if (Context.Request.Url.Query.StartsWith("?sitemap"))
			{
				currentPage = 0;
				pageSize = Int32.MaxValue;
			}

			// set the sitemap properites
			e.Syndication.ChangeFrequency = ChangeFrequency.Daily;
			e.Syndication.Priority = 1.0F;

			// add links for the site
			e.Syndication.Links.Add(new Link(new Uri(this.SectionInformation.UrlPath.ToString() + "?feed"), "Coder Journal Atom Feed", LinkRelationship.Self, "application/atom+xml"));
			e.Syndication.Links.Add(new Link(this.SectionInformation.UrlPath, "Coder Journal", LinkRelationship.Alternate, "text/html"));

			// fill the feed
			Data.Journal.FillFeed(e.Syndication, currentPage, pageSize);

			base.OnLoadSyndication(e);
		}

		protected override void OnLoad(LoadModuleEventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
