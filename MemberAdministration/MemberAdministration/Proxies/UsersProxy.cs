using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MemberAdministration
{
	public class UsersProxy : IUsersProxy
	{
		readonly ILogger _logger;
		readonly IPhpCrudApiService _phpCrudApiService;

		List<User> _users;

		string uri = $"Users";

		public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
		{
			_logger = logger;
			_phpCrudApiService = phpCrudApiService;
		}

		public async Task AddNewUserAsync(string newUserName)
		{
			await GetUsersAsync();

			int nextID = 0;
			if (!(_users == null || _users.Count == 0))
			{
				var userNames = (from u in _users select u.Username.ToLower()).ToList();
				if (userNames.Contains(newUserName.ToLower())) return;

				var maxUserID = (from u in _users select u.UserID).Max();
				nextID = maxUserID + 1;
			}

			User newUser = new User();
			newUser.UserID = nextID;
			newUser.Active = true;
			newUser.Username = newUserName;

			await _phpCrudApiService.SendDataAsync(uri, newUser);

			await GetUsersAsync();
		}

		public async Task<List<User>> GetUsersAsync()
		{
			var tableResult = await _phpCrudApiService.GetDataAsync(uri);
			_users = _phpCrudApiService.GetList<User>(tableResult);

			_logger.LogInformation(_users.Count + " Users loaded");

			return _users;
		}
	}
}
