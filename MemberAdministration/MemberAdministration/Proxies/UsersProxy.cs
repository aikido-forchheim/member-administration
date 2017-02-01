using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace MemberAdministration
{
	public class UsersProxy : IUsersProxy
	{
		readonly ILogger _logger;
		readonly IPhpCrudApiService _phpCrudApiService;

		public UsersProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
		{
			_logger = logger;
			_phpCrudApiService = phpCrudApiService;
		}

		public async Task<List<User>> GetUsersAsync()
		{
			string uri = $"Users";
			var tableResult = await _phpCrudApiService.GetDataAsync(uri);
			var users = _phpCrudApiService.GetList<User>(tableResult);

			_logger.LogInformation(users.Count + " Users loaded");

			return users;
		}
	}
}
