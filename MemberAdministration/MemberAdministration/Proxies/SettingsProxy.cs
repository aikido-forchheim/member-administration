using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public class SettingsProxy : ISettingsProxy
	{
		public SettingsProxy()
		{
		}

		public Task<Setting> GetSetting(string key)
		{
			throw new NotImplementedException();
		}

		public Task<List<Setting>> GetSettings()
		{
			throw new NotImplementedException();
		}
	}
}
