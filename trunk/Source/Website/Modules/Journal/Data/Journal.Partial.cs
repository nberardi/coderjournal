using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using ManagedFusion.Syndication;

namespace CoderJournal.Modules.Journal.Data
{
	public partial class Journal
	{
		protected static Uri GetPostUrl(DateTime publishDate, string urlName)
		{
			return ManagedFusion.Common.Path.GetPortalUrl(String.Format("/{0}/{1}/{2}.aspx", publishDate.Year, publishDate.Month, urlName));
		}

		public static JournalCollection GetListByPage(int currentPage, int pageSize)
		{
			using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["ManagedFusion"].ConnectionString))
			{
				using (SqlCommand command = connection.CreateCommand())
				{
					command.CommandText = "GetPagedJournal";
					command.CommandType = CommandType.StoredProcedure;

					command.Parameters.AddWithValue("@CurrentPage", currentPage);
					command.Parameters.AddWithValue("@PageSize", pageSize);

					return FillCollection(command);
				}
			}
		}

		public static void FillFeed(Feed feed, int currentPage, int pageSize)
		{
			foreach (Journal entry in GetListByPage(currentPage, pageSize))
			{
				Entry e = new Entry(
					entry.Title,
					"urn:uuid:" + entry.JournalID.Value,
					entry.Updated
					);
				e.Content = new ManagedFusion.Syndication.Content("html", entry.Content);
				e.Summary = new Text("text", entry.Summary);
				e.Published = entry.Published;

				// the self link is added by default
				e.Links.Add(new ManagedFusion.Syndication.Link(GetPostUrl(entry.Published, entry.UrlName), entry.Title, LinkRelationship.Self, "html"));

				// add the links
				foreach (Link link in entry.ForeignLinks)
				{
					e.Links.Add(new ManagedFusion.Syndication.Link(
						new Uri(link.Href),
						link.Title,
						(LinkRelationship)Enum.Parse(typeof(LinkRelationship), link.Relationship),
						link.Type
						));
				}

				// add the tags
				foreach (Tag tag in entry.ForeignTags)
				{
					Category c = new Category(tag.Name);
					c.Label = tag.Description;
					e.Categories.Add(c);
				}

				// add authors
				foreach (User user in entry.Authors)
				{
					e.Authors.Add(new Person(user.Name, user.Email, new Uri(user.Link)));
				}

				// add contributors
				foreach (User user in entry.Contributors)
				{
					e.Contributors.Add(new Person(user.Name, user.Email, new Uri(user.Link)));
				}

				// add the entry
				feed.Items.Add(e);
			}
		}

		public TagCollection ForeignTags
		{
			get { return Tag.GetList("TagID in (select TagID from JournalTagLink where JournalID = '" + this.JournalID.Value + "')"); }
		}

		public UserCollection Authors
		{
			get { return User.GetList("UserID in (select UserID from JournalUserLink where EntryType = 'A' and JournalID = '" + this.JournalID.Value + "')"); }
		}

		public UserCollection Contributors
		{
			get { return User.GetList("UserID in (select UserID from JournalUserLink where EntryType = 'C' and JournalID = '" + this.JournalID.Value + "')"); }
		}
	}
}
