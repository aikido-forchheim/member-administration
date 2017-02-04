using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace MemberAdministration
{
	public partial class StartPage : ContentPage
	{
		void Handle_Completed(object sender, System.EventArgs e)
		{
			if ((this.BindingContext as StartPageViewModel).UserAdministrationCommand.CanExecute(null))
			{
				(this.BindingContext as StartPageViewModel).UserAdministrationCommand.Execute(null);
			}
		}

		public StartPage()
		{
			InitializeComponent();
		}
	}
}
