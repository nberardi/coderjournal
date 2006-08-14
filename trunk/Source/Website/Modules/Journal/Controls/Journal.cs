using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;

using ManagedFusion;
using ManagedFusion.Modules;
using ManagedFusion.Syndication;

namespace CoderJournal.Modules.Journal.Controls
{
	[
		ToolboxData(@"<{0}:Journal runat=""server"" />"),
		AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal),
		AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)
	]
	public class Journal : Control
	{
		public Journal()
		{
		}

		public List<Entry> Entries
		{
			get { return this.Module.Syndication.Items; }
		}

		/// <summary>A reference for <see cref="SiteInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SiteInfo SiteInformation { get { return SiteInfo.Current; } }

		/// <summary>A reference for <see cref="SectionInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected SectionInfo SectionInformation { get { return SectionInfo.Current; } }

		/// <summary>A reference for <see cref="CommunityInfo" />.</summary>
		/// <remarks>This is here for the convience of the module developer.</remarks>
		protected CommunityInfo CommunityInformation { get { return CommunityInfo.Current; } }

		/// <summary>Gets the properties for the current module.</summary>
		protected NameValueCollection Properties { get { return Module.Properties; } }

		/// <summary>Gets the current executing module.</summary>
		protected ModuleBase Module { get { return Common.ExecutingModule; } }

		protected override void Render(HtmlTextWriter writer)
		{
			// render <div id="journal">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "journal");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			for (int i = 0; i < Entries.Count; i++)
				RenderItem(i, writer);
				
			// render </div> for "journal"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();
		}

		protected void RenderItem(int index, HtmlTextWriter writer)
		{
			Entry entry = Entries[index];
			CustomField journalID = entry.GetCustomField("cj", "id");

			if (journalID == null)
				throw new ApplicationException("<cj:id /> must be present in the feed.");

			string titleUrl = null;

			// find the link to the full contents of the entry
			foreach (Link link in entry.GetLinks(LinkRelationship.Self))
			{
				titleUrl = link.Href.ToString();
				break;
			}

			if (String.IsNullOrEmpty(titleUrl))
				throw new ApplicationException("Entry does not have a self referencing Link.");

			// render <div id="post">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			// render <div id="post-header">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-header-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-header");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			// render <h2 id="post-title">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-title-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-title");
			writer.RenderBeginTag(HtmlTextWriterTag.H2);

			// render title anchor
			writer.AddAttribute(HtmlTextWriterAttribute.Href, titleUrl);
			writer.AddAttribute(HtmlTextWriterAttribute.Rel, "bookmark");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, "View full posting of " + entry.Title);
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write(entry.Title);
			writer.RenderEndTag();

			// render </h2> of "post-title"
			writer.RenderEndTag();

			writer.WriteLine();
			writer.WriteLine();

			// write title content which include
			//   - publish date
			//   - categories
			//   - author
			writer.WriteLine("posted {0:D}", entry.Published);
			writer.Write("in ");

			// write categories
			foreach (Category category in entry.Categories)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-tag");
				writer.AddAttribute(HtmlTextWriterAttribute.Href, category.Term);
				writer.AddAttribute(HtmlTextWriterAttribute.Rel, "tag");
				writer.AddAttribute(HtmlTextWriterAttribute.Title, "View all posts for " + category.Label + " tag");
				writer.RenderBeginTag(HtmlTextWriterTag.A);
				writer.Write(category.Label);
				writer.RenderEndTag();
			}
			writer.WriteLine();

			writer.Write("by ");

			// write author
			if (entry.Authors.Count == 0)
				throw new ApplicationException("Entry does not have an Author.");

			string author = String.Empty;

			// verify that the author has a blog associated to them
			if (entry.Authors[0].Link != null)
			{
				writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-author");
				writer.AddAttribute(HtmlTextWriterAttribute.Href, entry.Authors[0].Link.ToString());
				writer.AddAttribute(HtmlTextWriterAttribute.Rel, "author");
				writer.AddAttribute(HtmlTextWriterAttribute.Title, "View blog for " + entry.Authors[0].Name);
				writer.RenderBeginTag(HtmlTextWriterTag.A);
			}

			// write the authors name
			author += entry.Authors[0].Name;

			// render the end of the anchor tag if the author has a blog
			if (entry.Authors[0].Link != null)
				writer.RenderEndTag();

			writer.WriteLine();
			writer.WriteLine();

			// render social bookmarks for this entry
			// render <div id="post-social-bookmarks">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-social-bookmarks-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-social-bookmarks");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			// render del.icio.us bookmark
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "social-bookmark delicious");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format("http://del.icio.us/post?v=2&amp;url={0}&amp;title={1}&amp;bodytext={2}", titleUrl, entry.Title, entry.Summary));
			writer.AddAttribute(HtmlTextWriterAttribute.Rel, "bookmark");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, "Post journal entry on del.icio.us");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("del.icio.us");
			writer.RenderEndTag();
			writer.WriteLine();

			// render digg bookmark
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "social-bookmark digg");
			writer.AddAttribute(HtmlTextWriterAttribute.Href, String.Format("http://digg.com/submit?phase=2&amp;url={0}&amp;title={1}&amp;bodytext={2}", titleUrl, entry.Title, entry.Summary));
			writer.AddAttribute(HtmlTextWriterAttribute.Rel, "bookmark");
			writer.AddAttribute(HtmlTextWriterAttribute.Title, "Post journal entry on digg");
			writer.RenderBeginTag(HtmlTextWriterTag.A);
			writer.Write("digg this");
			writer.RenderEndTag();

			// render </div> for "post-social-bookmarks"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();

			// render </div> for "post-header"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();

			writer.WriteLine();
			writer.WriteLine();

			// render <div id="post-summary">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-summary-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-summary");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			// render <div id="post-digg-button">
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-digg-button-" + journalID);
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.WriteLine();
			writer.Indent++;

			// render digg button script
			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.Write(@"digg_url = ""{0}"";", "nothing");
			writer.RenderEndTag();

			writer.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			writer.AddAttribute(HtmlTextWriterAttribute.Src, "http://digg.com/api/diggthis.js");
			writer.RenderBeginTag(HtmlTextWriterTag.Script);
			writer.RenderEndTag();

			// render </div> for "post-digg-button"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();

			writer.WriteLine();
			writer.WriteLine();

			writer.WriteLine(entry.Summary);

			// render </div> for "post-summary"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();

			// render <div id="post-footer" />
			writer.AddAttribute(HtmlTextWriterAttribute.Id, "post-footer-" + journalID);
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-footer");
			writer.RenderBeginTag(HtmlTextWriterTag.Div);
			writer.RenderEndTag();

			// render </div> for "post"
			writer.Indent--;
			writer.WriteLine();
			writer.RenderEndTag();

			writer.WriteLine();
			writer.WriteLine();

			// render <hr class="post-divider" />
			writer.AddAttribute(HtmlTextWriterAttribute.Class, "post-divider");
			writer.RenderBeginTag(HtmlTextWriterTag.Hr);
			writer.RenderEndTag();
		}
	}
}
