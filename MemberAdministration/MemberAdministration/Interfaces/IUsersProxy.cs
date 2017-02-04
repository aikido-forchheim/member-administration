using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface IUsersProxy
	{
		Task<List<User>> GetUsersAsync();

		Task AddNewUserAsync(string newUserName);
	}
}
