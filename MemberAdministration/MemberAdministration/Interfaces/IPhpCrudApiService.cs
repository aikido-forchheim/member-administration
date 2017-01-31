using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface IPhpCrudApiService
	{
		Task<string> GetDataAsync(string uri);

		List<T> GetList<T>(string tableResultJson);
	}
}
