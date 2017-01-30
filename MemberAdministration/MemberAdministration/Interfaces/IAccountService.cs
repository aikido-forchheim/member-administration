using System;
namespace MemberAdministration
{
	public interface IAccountService
	{
		bool IsRestApiAccountSet { get; set; }

		void StoreRestApiAccount(string apiUrl, string userName, string password);
	}
}
