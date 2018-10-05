using System;
using System.Collections.Generic;
using System.Text;
using Testinator.Server.DataAccess.Entities;

namespace Testinator.Server.Repositories.Settings
{
    public interface ISettingsRepository
    {
        Setting GetSetting(string Key);
    }
}
