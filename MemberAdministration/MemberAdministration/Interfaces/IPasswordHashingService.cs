using System;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface IPasswordHashingService
	{
		Task<bool> IsValidAsync(string enteredPassword, string storedPasswordHash);

		Task<string> HashPasswordAsync(string password, byte[] saltBytes = null);
	}
}
