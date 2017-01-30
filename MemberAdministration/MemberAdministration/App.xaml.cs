﻿using Prism.Unity;
using Microsoft.Practices.Unity;
using Xamarin.Forms;
using Microsoft.Extensions.Logging;

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

			Container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
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