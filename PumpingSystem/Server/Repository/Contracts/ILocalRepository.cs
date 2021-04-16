using PumpingSystem.Common;

namespace PumpingSystem.Server.Repository
{
    public interface ILocalRepository
    {
        ProcessChart ProcessChart { get; set; }
        Pump Pump { get; set; }
        WaterTank[] Tanks { get; set; }
    }
}
