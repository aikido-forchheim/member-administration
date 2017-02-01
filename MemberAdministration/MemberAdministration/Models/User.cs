using System;
namespace MemberAdministration
{
	public class User
	{
		public User()
		{
		}

		public int UserID
		{
			get;
			set;
		}

		public string Username
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public bool Active
		{
			get;
			set;
		}
	}
}
