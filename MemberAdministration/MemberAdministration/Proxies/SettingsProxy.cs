using System;
using System.Linq;
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

		List<Setting> _settings = null;

		public SettingsProxy(ILogger logger, IPhpCrudApiService phpCrudApiService)
		{
			_logger = logger;
			_phpCrudApiService = phpCrudApiService;
		}

		public async Task<Setting> GetSettingAsync(string key)
		{
			if (_settings == null) await GetSettingsAsync();

			var setting = (from s in _settings where s.Key == key select s).SingleOrDefault();

			return setting;
		}

		public async Task<List<Setting>> GetSettingsAsync()
		{
			if (_settings != null) return _settings;

			string uri = $"Settings";
			var tableResult = await _phpCrudApiService.GetDataAsync(uri);
			_settings = _phpCrudApiService.GetList<Setting>(tableResult);

			return _settings;
		}
	}
}
