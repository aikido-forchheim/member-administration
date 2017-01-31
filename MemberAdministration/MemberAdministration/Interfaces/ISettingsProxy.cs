using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface ISettingsProxy
	{
		Task<Setting> GetSettingAsync(string key);

		Task<List<Setting>> GetSettingsAsync();
	}
}
