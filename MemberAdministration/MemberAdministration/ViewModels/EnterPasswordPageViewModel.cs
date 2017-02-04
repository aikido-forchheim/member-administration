using System;
using System.Windows.Input;
using Prism.Commands;

namespace MemberAdministration
{
	public class EnterPasswordPageViewModel
	{
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

		public EnterPasswordPageViewModel()
		{
			SavePasswordCommand = new DelegateCommand<object>(this.OnSavePassword, this.CanSavePassword);
		}

		bool CanSavePassword(object arg)
		{
			return Password == PasswordRepeat && !string.IsNullOrWhiteSpace(Password) && Password.Length >= 8;
		}

		void OnSavePassword(object obj)
		{
			//
		}
}
}
