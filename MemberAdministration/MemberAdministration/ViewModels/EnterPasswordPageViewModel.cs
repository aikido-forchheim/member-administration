using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Navigation;

namespace MemberAdministration
{
	public class EnterPasswordPageViewModel : INavigationAware
	{
		User _user;

		string _password;
		public string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;

				(SavePasswordCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
			}
		}

		string _passwordRepeat;
		public string PasswordRepeat
		{
			get
			{
				return _passwordRepeat;
			}
			set
			{
				_passwordRepeat = value;

				(SavePasswordCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
			}
		}

		public ICommand SavePasswordCommand { get; private set; }

		readonly IPasswordHashingService _passwordHashingService;
		readonly IUsersProxy _usersProxy;
		readonly INavigationService _navigationService;

		public EnterPasswordPageViewModel(IPasswordHashingService passwordHashingService, IUsersProxy usersProxy, INavigationService navigationService)
		{
			_passwordHashingService = passwordHashingService;
			_usersProxy = usersProxy;
			_navigationService = navigationService;

			SavePasswordCommand = new DelegateCommand<object>(this.OnSavePassword, this.CanSavePassword);
		}

		bool CanSavePassword(object arg)
		{
			return Password == PasswordRepeat && !string.IsNullOrWhiteSpace(Password) && Password.Length >= 8;
		}

		async void OnSavePassword(object obj)
		{
			string passwordHash = await _passwordHashingService.HashPasswordAsync(_password);

			_user.Password = passwordHash;

			await _usersProxy.UpdateUserAsync(_user);

			await _navigationService.NavigateAsync(nameof(StartPage));
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			//throw new NotImplementedException();
		}

		public void OnNavigatedTo(NavigationParameters parameters)
		{
			if (parameters != null && parameters.Count > 0)
			{
				_user = parameters[nameof(User)] as User;
			}
		}

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }
    }
}
