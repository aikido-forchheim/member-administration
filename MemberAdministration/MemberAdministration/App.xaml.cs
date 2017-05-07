using Prism.Unity;
using Microsoft.Practices.Unity;
using Xamarin.Forms;
using Microsoft.Extensions.Logging;
using MemberAdministration.Views;

namespace MemberAdministration
{
	public partial class App : PrismApplication
	{
		public const string AppId = "10bc9068-17ac-4f0f-a596-7fdfe20bc9f4";

		public App(IPlatformInitializer initializer = null) : base(initializer)
		{

		}

		protected override void OnInitialized()
		{
			InitializeComponent();

			NavigationService.NavigateAsync("MainPage");
		}

		protected override void RegisterTypes()
		{
			ILoggerFactory loggerFactory = new LoggerFactory();
			ILogger logger = loggerFactory.CreateLogger<App>();
			Container.RegisterInstance(loggerFactory);
			Container.RegisterInstance(logger);

			Container.RegisterTypeForNavigation<MainPage>();
			Container.RegisterTypeForNavigation<StartPage>();
			Container.RegisterTypeForNavigation<RestApiSettingsPage>();
			Container.RegisterTypeForNavigation<UserAdministrationPage>();
			Container.RegisterTypeForNavigation<EnterPasswordPage>();

			Container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
			Container.RegisterType<IPhpCrudApiService, PhpCrudApiService>(new ContainerControlledLifetimeManager());
			Container.RegisterType<ISettingsProxy, SettingsProxy>(new ContainerControlledLifetimeManager());

			Container.RegisterType<IPasswordHashingService, PasswordHashingService>();
			Container.RegisterType<IUsersProxy, UsersProxy>();
        }

		//protected override void OnStart()
		//{
		//	// Handle when your app starts
		//}

		//protected override void OnSleep()
		//{
		//	// Handle when your app sleeps
		//}

		//protected override void OnResume()
		//{
		//	// Handle when your app resumes
		//}
	}
}