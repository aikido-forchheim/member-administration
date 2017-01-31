using System;
using Xamarin.Auth;

namespace MemberAdministration
{
	public interface IAccountService
	{
		bool IsRestApiAccountSet { get; set; }

		void StoreRestApiAccount(string apiUrl, string userName, string password);

		RestApiAccount RestApiAccount { get; set; }
	}
}
