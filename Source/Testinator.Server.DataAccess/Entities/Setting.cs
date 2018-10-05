using Testinator.Server.DataAccess.Entities.Base;

namespace Testinator.Server.DataAccess.Entities
{
    public class Setting : BaseObject<int>
    {    
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
