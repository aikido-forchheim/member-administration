using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MemberAdministration
{
	public interface ISettingsProxy
	{
		Task<Setting> GetSetting(string key);

		Task<List<Setting>> GetSettings();
	}
}
