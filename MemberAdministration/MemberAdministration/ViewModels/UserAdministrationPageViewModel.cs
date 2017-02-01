using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

		public UserAdministrationPageViewModel(IUsersProxy usersProxy)
		{
			_usersProxy = usersProxy;
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
