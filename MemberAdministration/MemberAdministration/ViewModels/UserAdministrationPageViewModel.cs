using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;

namespace MemberAdministration
{
	public class UserAdministrationPageViewModel : BindableBase, INavigationAware
	{
		readonly IUsersProxy _usersProxy;

		List<User> _users;
		public List<User> Users
		{
			get
			{
				return _users;
			}
			set
			{
				SetProperty(ref _users, value);
			}
		}

		string _newUserName;

		public string NewUserName
		{
			get
			{
				return _newUserName;
			}
			set
			{
				SetProperty(ref _newUserName, value);

				(AddNewUserCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
			}
		}

		public ICommand AddNewUserCommand { get; private set; }

		public UserAdministrationPageViewModel(IUsersProxy usersProxy)
		{
			_usersProxy = usersProxy;

			AddNewUserCommand = new DelegateCommand<object>(this.OnAddNewUser, this.CanStartAddNewUser);
		}

		bool CanStartAddNewUser(object arg)
		{
			if (_users == null) return true;

			if (string.IsNullOrWhiteSpace(_newUserName)) return false;

			var username = (from u in _users where u.Username.ToLower() == _newUserName.ToLower() select u.Username).SingleOrDefault();

			bool userAlreadyExists = false;

			if (!string.IsNullOrEmpty(username)) userAlreadyExists = true;

			return !userAlreadyExists;
		}

		async void OnAddNewUser(object obj)
		{
			await _usersProxy.AddNewUserAsync(_newUserName);

			Users = await _usersProxy.GetUsersAsync();
			(AddNewUserCommand as DelegateCommand<object>).RaiseCanExecuteChanged();
		}

		public void OnNavigatedFrom(NavigationParameters parameters)
		{
			//throw new NotImplementedException();
		}

		public async void OnNavigatedTo(NavigationParameters parameters)
		{
			Users = await _usersProxy.GetUsersAsync();
		}
	}
}
