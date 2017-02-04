using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface IPhpCrudApiService
	{
		Task<string> GetDataAsync(string uri, bool serverTransform = false);

		Task<T> GetDataAsync<T>(string uri);

		List<T> GetList<T>(string tableResultJson);

		Task<string> SendDataAsync<T>(string url, T dataObject);
	}
}
