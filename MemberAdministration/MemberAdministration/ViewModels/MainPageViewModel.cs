using System;
using System.Linq;
using Xamarin.Auth;

namespace MemberAdministration
{
	public class MainPageViewModel
	{
		private readonly IAccountService _accountService;

		public MainPageViewModel(IAccountService accountService)
		{
			_accountService = accountService;
		}

		public bool IsRestApiAccountSet
		{
			get
			{
				return _accountService.IsRestApiAccountSet;
			}
			set
			{
			}
		}


	}
}
