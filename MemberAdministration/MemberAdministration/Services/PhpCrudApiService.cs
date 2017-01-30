using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public class PhpCrudApiService
	{
		public PhpCrudApiService()
		{
		}

		CookieContainer cc = new CookieContainer();

		public async Task<string> GetTokenAsync(string apiUrl, string userName, string password)
		{
			HttpWebRequest req = (HttpWebRequest)WebRequest.Create(apiUrl);
			//req.ContentType = contentType;
			req.Method = "POST";

			req.CookieContainer = cc;

			var postData = $"username={userName}&password={password}";
			var data = Encoding.UTF8.GetBytes(postData);

			req.ContentType = "application/x-www-form-urlencoded";
			//req. = data.Length;

			using (var stream = await Task.Factory.FromAsync<Stream>(req.BeginGetRequestStream, req.EndGetRequestStream, null))
			{
				stream.Write(data, 0, data.Length);
			}

			WebResponse response = await req.GetResponseAsync();
			Stream responseStream = response.GetResponseStream();

			// Do whatever you need with the response
			Byte[] myData = ReadFully(responseStream);
			string responseString = Encoding.UTF8.GetString(myData, 0, myData.Length);

			return responseString;
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
