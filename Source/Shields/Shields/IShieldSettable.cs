using System.Collections.Generic;

namespace FrontierDevelopments.Shields
{
    public interface IShieldSettable
    {
        IEnumerable<ShieldSetting> ShieldSettings { get; set; }
        bool HasWantSettings { get; }
        void ClearWantSettings();
        void NotifyWantSettings();
    }
}