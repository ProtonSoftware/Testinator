using Microsoft.EntityFrameworkCore;
using System.Linq;
using Testinator.Server.DataAccess;
using Testinator.Server.DataAccess.Entities;
using Testinator.Server.Repositories.Base;

namespace Testinator.Server.Repositories.Settings
{
    public class SettingsRepository : BaseRepository<Setting, int>, ISettingsRepository
    {
        #region Setup

        public SettingsRepository(TestinatorAppDataContext context) : base(context) { }

        protected override DbSet<Setting> DbSet => Db.Settings;

        #endregion

        public Setting GetSetting(string Key)
        {
            return DbSet.Where(x => x.Key == Key).FirstOrDefault();
        }
    }
}
