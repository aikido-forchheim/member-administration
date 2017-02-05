using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MemberAdministration
{
	class UsersWrapper
	{
		public List<User> Users
		{
			get;
			set;
		}
	}

	public class UsersProxy : IUsersProxy
	{
		readonly ILogger _logger;
		readonly IPhpCrudApiService _phpCrudApiService;

		List<User> _users;

		string _uri = $"Users";

		public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
		{
			_logger = logger;
			_phpCrudApiService = phpCrudApiService;
		}

		public async Task UpdateUserAsync(User user)
		{
			UserBase u = new UserBase();
			u.Active = user.Active;
			u.Password = user.Password;
			u.Username = user.Username;

			await _phpCrudApiService.UpdateDataAsync(_uri+"/"+user.UserID.ToString(), user);

			await GetUsersAsync();
		}

		public async Task AddNewUserAsync(string newUserName)
		{
			await GetUsersAsync();

			int nextID = 1;
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

			await _phpCrudApiService.SendDataAsync(_uri, newUser);

			await GetUsersAsync();
		}

		public async Task<User> GetUserAsync(string username)
		{
			string uri = $"{_uri}?filter=Username,eq,{username}";

			_users = (await _phpCrudApiService.GetDataAsync<UsersWrapper>(uri)).Users;

			var user = _users.SingleOrDefault();

			return user;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			_users = (await _phpCrudApiService.GetDataAsync<UsersWrapper>(_uri)).Users;

			_logger.LogInformation(_users.Count + " Users loaded");

			return _users;
		}
	}
}
