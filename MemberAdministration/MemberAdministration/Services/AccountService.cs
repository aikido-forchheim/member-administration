using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Xamarin.Auth;

namespace MemberAdministration
{
	public class AccountService : IAccountService
	{
		private readonly ILogger _logger;

		public AccountService(ILogger logger)
		{
			_logger = logger;
		}

		public bool IsRestApiAccountSet
		{
			get
			{
				try
				{
					var account = AccountStore.Create().FindAccountsForService(App.AppId)?.FirstOrDefault();
					return (account != null) ? true : false;
				}
				catch (Exception ex)
				{
					_logger.LogError(ex.ToString());
					return false;
				}
			}
			set
			{
			}
		}

		public void StoreRestApiAccount(string apiUrl, string userName, string password)
		{
			if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
			{
				Account account = new Account
				{
					Username = userName
				};
				account.Properties.Add("Password", password);
				account.Properties.Add("ApiUrl", apiUrl);
				AccountStore.Create().Save(account, App.AppId);
			}
		}
	}
}
