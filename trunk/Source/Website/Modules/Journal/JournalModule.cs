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
			string daysToGetString = Context.Request.QueryString["days"];
			int daysToGet = 3;

			// this statement does three things, first it checks to see if there was a value in the query string
			// second it validates and sets the int if it is a valid int
			// third it checks to see if "all" is set so that it returns the whole journal database
			if (String.IsNullOrEmpty(daysToGetString) == false
				&& Int32.TryParse(daysToGetString, out daysToGet) == false
				&& daysToGetString.ToLower() == "all")
			{
				daysToGet = Int32.MaxValue;
			}


			base.OnLoadSyndication(e);
		}

		protected override void OnLoad(LoadModuleEventArgs e)
		{
			base.OnLoad(e);
		}
	}
}
