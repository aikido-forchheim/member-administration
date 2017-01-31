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

			_account = AccountStore.Create().FindAccountsForService(App.AppId)?.FirstOrDefault();

			_restApiAccount = new RestApiAccount() { ApiUrl = _account?.Properties["ApiUrl"], Password = _account?.Properties["Password"], UserName = _account.Username };
		}

		Account _account;
		RestApiAccount _restApiAccount;
		public RestApiAccount RestApiAccount
		{
			get
			{
				return _restApiAccount;
			}
			set
			{
			}
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

				var accounts = AccountStore.Create().FindAccountsForService(App.AppId).ToList();
				foreach (var a in accounts)
				{
					AccountStore.Create().Delete(a, App.AppId);
				}

				AccountStore.Create().Save(account, App.AppId);
			}
		}
	}
}
