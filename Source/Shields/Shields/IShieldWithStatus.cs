using System.Collections.Generic;

namespace FrontierDevelopments.Shields
{
    public interface IShieldWithStatus
    {
        IEnumerable<IShieldStatus> Status { get; }
        string TextStats { get; }
    }
}