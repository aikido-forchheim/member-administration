using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Logging;

namespace MemberAdministration
{
	public class PhpCrudApiService : IPhpCrudApiService
	{
		private readonly IAccountService _accountService;
		private readonly ILogger _logger;

		CookieContainer _cookieContainer = new CookieContainer();

		string _token;

		public PhpCrudApiService(ILogger logger, IAccountService accountService)
		{
			_logger = logger;
			_accountService = accountService;
		}

		public async Task<string> GetDataAsync(string uri)
		{
			if (_token == null) _token = await GetTokenAsync();

			string fullUri = _accountService.RestApiAccount.ApiUrl + uri;
			fullUri = AddCsrfToken(fullUri);

			HttpWebRequest httpWebRequest = HttpWebRequestWithCookieContainer(fullUri);
			httpWebRequest.Method = "GET";

			return await GetStringAsync(httpWebRequest);
		}

		public List<T> GetList<T>(string tableResultJson)
		{
			var result = JObject.Parse(tableResultJson);

			var recordsArray = result.First.First.SelectToken("records").ToList();
			var columnsArray = result.First.First.SelectToken("columns").ToList();

			List<T> list = new List<T>();
			foreach (var columnValuesArray in recordsArray)
			{
				JObject jObject = new JObject();

				for (int i = 0; i < columnsArray.Count; i++)
				{
					string columnName = columnsArray[i].Value<string>();
					JProperty jProperty = new JProperty(columnName, columnValuesArray[i]);
					jObject.Add(jProperty);
				}

				var reorganizedJson = jObject.ToString();

				T deserializedObject = JsonConvert.DeserializeObject<T>(reorganizedJson);
				list.Add(deserializedObject);
			}

			return list;
		}

		string AddCsrfToken(string fullUri)
		{
			if (!fullUri.Contains("?")) fullUri = fullUri + $"?csrf={_token}";
			else fullUri = fullUri + $"&csrf={_token}";
			return fullUri;
		}

		async Task<string> GetTokenAsync()
		{
			return await GetTokenAsync(_accountService.RestApiAccount.ApiUrl, _accountService.RestApiAccount.UserName, _accountService.RestApiAccount.Password);
		}

		async Task<string> GetTokenAsync(string apiUrl, string userName, string password)
		{
			try
			{
				HttpWebRequest httpWebReqeust = HttpWebRequestWithCookieContainer(apiUrl);

				httpWebReqeust.Method = "POST";

				var postData = $"username={userName}&password={password}";

				var data = Encoding.UTF8.GetBytes(postData);

				httpWebReqeust.ContentType = "application/x-www-form-urlencoded";

				using (var stream = await Task.Factory.FromAsync<Stream>(httpWebReqeust.BeginGetRequestStream, httpWebReqeust.EndGetRequestStream, null))
				{
					stream.Write(data, 0, data.Length);
				}

				return await GetStringAsync(httpWebReqeust);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		HttpWebRequest HttpWebRequestWithCookieContainer(string url)
		{
			HttpWebRequest httpWebReqeust = (HttpWebRequest)WebRequest.Create(url);
			httpWebReqeust.CookieContainer = _cookieContainer;
			return httpWebReqeust;
		}

		async Task<string> GetStringAsync(HttpWebRequest httpWebRequest)
		{
			try
			{
				WebResponse response = await httpWebRequest.GetResponseAsync();
				Stream responseStream = response.GetResponseStream();

				byte[] responseBytes = ReadFully(responseStream);
				string responseString = Encoding.UTF8.GetString(responseBytes, 0, responseBytes.Length);

				return responseString;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex.ToString());
				throw;
			}
		}

		byte[] ReadFully(Stream stream)
		{
			byte[] buffer = new byte[32768];
			using (MemoryStream ms = new MemoryStream())
			{
				while (true)
				{
					int read = stream.Read(buffer, 0, buffer.Length);
					if (read <= 0)
						return ms.ToArray();
					ms.Write(buffer, 0, read);
				}
			}
		}
	}
}
