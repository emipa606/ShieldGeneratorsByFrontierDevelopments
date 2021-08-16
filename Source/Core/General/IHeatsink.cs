namespace FrontierDevelopments.General
{
    public interface IHeatsink
    {
        bool OverTemperature { get; }
        float Temp { get; }
        bool WantThermalShutoff { get; set; }
        bool ThermalShutoff { get; }
        void PushHeat(float wattDays);
    }
}