using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace MemberAdministration
{
	public partial class UserAdministrationPage : ContentPage
	{
		async void Handle_Clicked(object sender, System.EventArgs e)
		{
			if ((this.BindingContext as UserAdministrationPageViewModel).AddNewUserCommand.CanExecute(null))
			{
				var answer = await DisplayAlert("Neuen Benutzer hinzufügen?", $"Möchten Sie wirklich '{entryNewUserName.Text}' als Benutzer hinzufügen?", "Ja", "Nein");

				if (answer == true)
				{
					(this.BindingContext as UserAdministrationPageViewModel).AddNewUserCommand.Execute(null);
				}
			}
			else
			{
				await DisplayAlert("Benutzer bereits vorhanden!", "Es ist bereits ein Benutzer mit dem Namen '" + entryNewUserName.Text + "' vorhanden.", "OK");
			}
		}

		readonly ILogger _logger;

		public UserAdministrationPage(ILogger logger)
		{
			_logger = logger;

			try
			{
				InitializeComponent();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
			}
		}
	}
}
