namespace FrontierDevelopments.Shields
{
    public interface IShieldStatus
    {
        bool Online { get; }
        string Description { get; }
    }
}