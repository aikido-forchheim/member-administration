using NUnit.Framework;
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using Org.BouncyCastle.Security;
using PCLCrypto;

namespace MemberAdministration.NUnitTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase2()
		{
			int saltByteSize = 32;
			int iterations = 10000;
			int keyLengthInBytes = 20; //https://www.owasp.org/index.php/Using_Rfc2898DeriveBytes_for_PBKDF2 => With PBKDF2-SHA1 this is 160 bits or 20 bytes
			string hashAlgorithm = "SHA512";
			char delimiter = ':';

			string pepper = "addMeToPassword";

			DateTime start = DateTime.Now;

			var secureRandom = SecureRandom.GetInstance($"{hashAlgorithm}PRNG", true);

			var saltBytes = new byte[saltByteSize];
			secureRandom.NextBytes(saltBytes);

			string salt = Convert.ToBase64String(saltBytes);

			Console.WriteLine();

			string password = "hallo";
				
			//uses Pbkdf2
			byte[] deriveBytes = NetFxCrypto.DeriveBytes.GetBytes(password+pepper, saltBytes, iterations, keyLengthInBytes);

			string hash = Convert.ToBase64String(deriveBytes);

			string stringToStore = $"{hash}{delimiter}{salt}";

			TimeSpan duration = DateTime.Now - start;

			Console.WriteLine(duration.TotalMilliseconds.ToString());

			Console.WriteLine(stringToStore);
		}

		public void TestCase()
		{
			var tableResult = @"{ ""Settings"": {
									""columns"": [""Key"", ""Value"", ""Des""],
									""records"": [[""HashAlgorithm"", ""SHA512"", ""HashAlgorithm used for password hashing""], [""Logo"", ""https:\/\/www.yourdomain.com\/yourlogo.jpg"", ""Vereinslogo""]]
										}
									}";

            Setting sampleSetting = new Setting()
            {
                Key = "HashAlgorithm",
                Value = "SHA512",
                Des = "HashAlgorithm used for password hashing"
            };

            //string sinlgeObject = JsonConvert.SerializeObject(sampleSetting);

            var result = JObject.Parse(tableResult);

			var recordsArray = result.First.First.SelectToken("records").ToList();
			var columnsArray = result.First.First.SelectToken("columns").ToList();

			List<Setting> list = new List<Setting>();
			foreach (var columnValuesArray in recordsArray)
			{
				JObject o = new JObject();

				for (int i = 0; i < columnsArray.Count; i++)
				{
					string columnName = columnsArray[i].Value<string>();
					JProperty p = new JProperty(columnName, columnValuesArray[i]);
					o.Add(p);
				}

				var str = o.ToString();

				Setting s = JsonConvert.DeserializeObject<Setting>(str);
				list.Add(s);
			}
		}
	}
}
