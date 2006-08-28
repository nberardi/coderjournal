using System;
using System.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace CoderJournal.Security
{
	public class JournalRoleProvider : RoleProvider
	{
		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string ApplicationName
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public override void CreateRole(string roleName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override string[] GetAllRoles()
		{
			return new List<string>(ManagedFusion.SectionInfo.Current.Roles.Keys).ToArray();
		}

		public override string[] GetRolesForUser(string username)
		{
			return new string[] { ManagedFusion.Security.PortalRole.NotAuthenticated.ToString() };
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			if (roleName.ToLower() == ManagedFusion.Security.PortalRole.Everybody.ToString().ToLower())
				return true;

			if (username.ToLower() == roleName.ToLower())
				return true;

			if (roleName.ToLower() == ManagedFusion.Security.PortalRole.Authenticated.ToString().ToLower() && String.IsNullOrEmpty(username) == false)
				return true;

			if (roleName.ToLower() == ManagedFusion.Security.PortalRole.NotAuthenticated.ToString().ToLower()
				&& username.ToLower() == ManagedFusion.Security.PortalRole.NotAuthenticated.ToString().ToLower())
				return true;

			return false;
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public override bool RoleExists(string roleName)
		{
			return ManagedFusion.SectionInfo.Current.Roles.ContainsKey(roleName);
		}
	}
}
