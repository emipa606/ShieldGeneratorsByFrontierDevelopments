namespace FrontierDevelopments.General.Comps
{
    public interface IFlickBoardSwitch
    {
        bool WantFlick { get; }
        void Notify_Flicked();
    }
}