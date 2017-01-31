using System;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace MemberAdministration
{
	public class StartPageViewModel : BindableBase
	{
		readonly ISettingsProxy _settingsProxy;

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


		public StartPageViewModel(ISettingsProxy settingsProxy)
		{
			_settingsProxy = settingsProxy;

			InitLogoAsync();
		}

		async Task InitLogoAsync()
		{
			var logoSetting = await _settingsProxy.GetSettingAsync("Logo");

			ImageSource = logoSetting.Value;
		}
	}
}
