using System;
using System.Data;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CoderJournal.Modules.Journal.Controls.Design
{
	public class JournalDesigner : ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			return @"
<div id=""journal"">
	<div id=""post-1"" class=""post"">
		<div id=""post-header-1"" class=""post-header"">
			<h2 id=""post-title-1"" class=""post-title""><a href=""http://www.coderjournal.com"" rel=""bookmark"" title=""View full posting of Journal Title"">Journal Title</a></h2>

			posted Tuesday, March 14, 2006
			in <a class=""post-tag"" href=""http://www.coderjournal.com/Default.aspx/tag1"" rel=""tag"" title=""View all posts for tag1 tag"">tag1</a> <a class=""post-tag"" href=""http://www.coderjournal.com/Default.aspx/tag2"" rel=""tag"" title=""View all posts for tag2 tag"">tag2</a>
			by <a class=""post-author"" href=""http://www.coderjournal.com/Default.aspx/jon"" rel=""author"" title=""View blog for Jon"">Jon</a>

			<div id=""post-social-bookmarks-1"" class=""post-social-bookmarks"">
				<a class=""social-bookmark delicious"" href=""http://del.icio.us"" rel=""bookmark"" title=""Post journal entry on del.icio.us"">del.icio.us</a>
				<a class=""social-bookmark digg"" href=""http://digg.com"" rel=""bookmark"" title=""Post journal entry on digg"">digg this</a>
			</div>
		</div>

		<div id=""post-summary-1"" class=""post-summary"">
			<div id=""post-digg-button-1"">
				<script type=""text/javascript""> digg_url = 'URLOFSTORY'; </script>
				<script type=""text/javascript"" src=""http://digg.com/api/diggthis.js""></script>
			 </div>

			Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec eu elit. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas dapibus rutrum tortor. Praesent fermentum lobortis turpis. Curabitur ligula lorem, pharetra non, lobortis eu, accumsan quis, augue. Integer nec nibh feugiat ligula hendrerit convallis. Integer ut augue ut eros lobortis hendrerit. Donec porttitor turpis sit amet sapien. Donec sollicitudin euismod lorem. Etiam lorem ipsum, suscipit eu, vulputate quis, tempus vel, nulla. Vestibulum laoreet. Etiam purus. Integer laoreet ligula nec mauris. Suspendisse potenti. Praesent venenatis mauris id massa. Mauris egestas. Quisque tempus ligula sagittis augue.
		</div>

		<div id=""post-footer-1"" class=""post-footer"">
		</div>
	</div>

	<hr class=""post-divider"" />

	<div id=""post-2"" class=""post"">
		<div id=""post-header-2"" class=""post-header"">
			<h2 id=""post-title-2"" class=""post-title""><a href=""http://www.coderjournal.com"" rel=""bookmark"" title=""View full posting of Journal Title 2"">Journal Title 2</a></h2>

			posted Tuesday, March 14, 2006
			in <a class=""post-tag"" href=""http://www.coderjournal.com/Default.aspx/tag1"" rel=""tag"" title=""View all posts for tag1 tag"">tag1</a> <a class=""post-tag"" href=""http://www.coderjournal.com/Default.aspx/tag2"" rel=""tag"" title=""View all posts for tag2 tag"">tag2</a>
			by <a class=""post-author"" href=""http://www.coderjournal.com/Default.aspx/jon"" rel=""author"" title=""View blog for Jon"">Jon</a>

			<div id=""post-social-bookmarks-2"" class=""post-social-bookmarks"">
				<a class=""social-bookmark delicious"" href=""http://del.icio.us"" rel=""bookmark"" title=""Post journal entry on del.icio.us"">del.icio.us</a>
				<a class=""social-bookmark digg"" href=""http://digg.com"" rel=""bookmark"" title=""Post journal entry on digg"">digg this</a>
			</div>
		</div>

		<div id=""post-summary-2"" class=""post-summary"">
			<div id=""post-digg-button-2"">
				<script type=""text/javascript""> digg_url = 'URLOFSTORY'; </script>
				<script type=""text/javascript"" src=""http://digg.com/api/diggthis.js""></script>
			 </div>

			Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Donec eu elit. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas dapibus rutrum tortor. Praesent fermentum lobortis turpis. Curabitur ligula lorem, pharetra non, lobortis eu, accumsan quis, augue. Integer nec nibh feugiat ligula hendrerit convallis. Integer ut augue ut eros lobortis hendrerit. Donec porttitor turpis sit amet sapien. Donec sollicitudin euismod lorem. Etiam lorem ipsum, suscipit eu, vulputate quis, tempus vel, nulla. Vestibulum laoreet. Etiam purus. Integer laoreet ligula nec mauris. Suspendisse potenti. Praesent venenatis mauris id massa. Mauris egestas. Quisque tempus ligula sagittis augue.
		</div>

		<div id=""post-footer-2"" class=""post-footer"">
		</div>
	</div>

	<hr class=""post-divider"" />

</div>";
		}
	}
}
