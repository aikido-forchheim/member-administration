using MemberAdministration.Views;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Auth;

namespace MemberAdministration
{
	public class MainPageViewModel
	{
        public ICommand SettingsCommand { get; private set; }

        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;

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

		public MainPageViewModel(IAccountService accountService, INavigationService navigationService)
		{
			_accountService = accountService;
            _navigationService = navigationService;

            SettingsCommand = new DelegateCommand<object>(this.OnSettings, this.CanSettings);
		}

        private void OnSettings(object state)
        {
            _navigationService.NavigateAsync(nameof(RestApiSettingsPage));
        }
        private bool CanSettings(object state)
        {
            return true;
        }
	}
}
