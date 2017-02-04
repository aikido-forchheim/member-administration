using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Extensions.Options;

namespace MemberAdministration
{
	class SettingsWrapper
	{
		public List<Setting> Settings
		{
			get;
			set;
		}
	}

	public class SettingsProxy : ISettingsProxy
	{
		readonly ILogger _logger;
		readonly IPhpCrudApiService _phpCrudApiService;

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

			var tableResult = await _phpCrudApiService.GetDataAsync(uri, true);
			//_settings = _phpCrudApiService.GetList<Setting>(tableResult);

			//_settings = JsonConvert.DeserializeObject<List<Setting>>(tableResult);



			var wrapper =  JsonConvert.DeserializeObject<SettingsWrapper>(tableResult);

			_settings = wrapper.Settings;

			_logger.LogInformation(_settings.Count + " Settings loaded");

			return _settings;
		}
	}
}
