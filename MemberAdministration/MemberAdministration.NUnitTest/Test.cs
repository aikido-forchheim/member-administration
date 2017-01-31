using NUnit.Framework;
using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace MemberAdministration.NUnitTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
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
