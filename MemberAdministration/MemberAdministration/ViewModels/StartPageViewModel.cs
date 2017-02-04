using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace MemberAdministration
{
	public class StartPageViewModel : BindableBase, INavigationAware
	{
		readonly ISettingsProxy _settingsProxy;
		readonly IAccountService _accountService;
		readonly INavigationService _navigationService;
		readonly IUsersProxy _usersProxy;

		public string Title
		{
			get;
			set;
		} = "Mitgliederverwaltung";

		string _imageSource;
		public string ImageSource
		{
			get
			{
				return _imageSource;
			}
			set
			{
				SetProperty(ref _imageSource, value);
			}
		}

		string _restApiAccountPassword;
		public string RestApiAccountPassword
		{
			get
			{
				return _restApiAccountPassword;
			}
			set
			{
				_restApiAccountPassword = value;
				(UserAdministrationCommand as DelegateCommand<object>)?.RaiseCanExecuteChanged();
			}
		}

		string _userName;
		public string UserName
		{
			get
			{
				return _userName;
			}
			set
			{
				SetProperty(ref _userName, value);

				(LoginCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
			}
		}

		string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				SetProperty(ref _password, value);
			}
		}

		bool _loginFailed;
		public bool LoginFailed
		{
			get
			{
				return _loginFailed;
			}
			set
			{
				SetProperty(ref _loginFailed, value);
			}
		}

		public ICommand LoginCommand { get; private set; }
		public ICommand UserAdministrationCommand { get; private set; }

		public StartPageViewModel(ISettingsProxy settingsProxy, IAccountService accountService, INavigationService navigationService, IUsersProxy usersProxy)
		{
			_settingsProxy = settingsProxy;
			_accountService = accountService;
			_navigationService = navigationService;
			_usersProxy = usersProxy;

			UserAdministrationCommand = new DelegateCommand<object>(this.OnUserAdministration, this.CanStartUserAdministration);
			LoginCommand = new DelegateCommand<object>(this.OnLogin, this.CanStartLogin);
		}

		bool CanStartLogin(object arg)
		{
			return !string.IsNullOrWhiteSpace(_userName);
		}

		async void OnLogin(object obj)
		{
			var serverUser = await _usersProxy.GetUserAsync(_userName);

			if (serverUser == null)
			{
				LoginFailed = true;
				return;
			}

			LoginFailed = !IsValid(_password, serverUser.Password);
		}

		bool IsValid(string plainTextPassword, string encryptedPassword)
		{
			
		}

		bool CanStartUserAdministration(object arg)
		{
			return _accountService.RestApiAccount.Password == RestApiAccountPassword;
		}

		void OnUserAdministration(object obj)
		{
			_navigationService.NavigateAsync(nameof(UserAdministrationPage));
		}

		async Task InitLogoAsync()
		{
			var logoSetting = await _settingsProxy.GetSettingAsync("Logo");

			ImageSource = logoSetting.Value;
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			//throw new NotImplementedException();
		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			await InitLogoAsync();
		}
	}
}
