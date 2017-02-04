using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Xamarin.Forms;

namespace MemberAdministration
{
	public partial class UserAdministrationPage : ContentPage
	{
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
