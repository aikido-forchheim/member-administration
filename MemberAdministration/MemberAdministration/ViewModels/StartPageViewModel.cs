using System;
using Prism.Mvvm;

namespace MemberAdministration
{
	public class StartPageViewModel : BindableBase
	{
		public string Title
		{
			get;
			set;
		} = "Mitgliederverwaltung";

		public StartPageViewModel()
		{
		}
	}
}
