using System;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface IPhpCrudApiService
	{
		Task<string> GetDataAsync(string uri);
	}
}
