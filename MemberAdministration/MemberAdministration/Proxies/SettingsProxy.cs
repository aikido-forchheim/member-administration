using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MemberAdministration
{
	public class SettingsProxy : ISettingsProxy
	{
		private readonly ILogger _logger;
		private readonly IPhpCrudApiService _phpCrudApiService;


		public SettingsProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
		{
			_logger = logger;
			_phpCrudApiService = phpCrudApiService;
		}

		public async Task<Setting> GetSettingAsync(string key)
		{
			throw new NotImplementedException();
		}

		public async Task<List<Setting>> GetSettingsAsync()
		{
			string uri = $"Settings";
			var tableResult = await _phpCrudApiService.GetDataAsync(uri);
			var result = JsonConvert.DeserializeObject(tableResult);


			return new List<Setting>();
		}
	}
}
