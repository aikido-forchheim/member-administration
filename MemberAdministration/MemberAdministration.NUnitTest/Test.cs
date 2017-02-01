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
			//all these value should be entered by the installer, so nobody knows what hashing parameters are used.
			//but if we need to change one of those values, all passwords need to be reentered
			//or we store them along the password like the salt or the iterations ???
			int saltByteSize = 512;
			int iterations = 1000;
			int keyLengthInBytes = 1024;
			string hashAlgorithm = "SHA512";
			char delimiter = ':';

			string pepper = "addMeToPassword";

			DateTime start = DateTime.Now;

			Random random = SecureRandom.GetInstance($"{hashAlgorithm}PRNG", true);

			var salt = new byte[saltByteSize];
			random.NextBytes(salt);

			string saltString = Convert.ToBase64String(salt);

			Console.WriteLine();

			string password = "hallo";
				
			byte[] deriveBytes = NetFxCrypto.DeriveBytes.GetBytes(password+pepper, salt, iterations, keyLengthInBytes);

			string stringToStore = $"{saltByteSize}{delimiter}{iterations}{delimiter}{keyLengthInBytes}{delimiter}{hashAlgorithm}{delimiter}{deriveBytes}{delimiter}{saltString}";

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

			Setting sampleSetting = new Setting();
			sampleSetting.Key = "HashAlgorithm";
			sampleSetting.Value = "SHA512";
			sampleSetting.Des = "HashAlgorithm used for password hashing";

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
