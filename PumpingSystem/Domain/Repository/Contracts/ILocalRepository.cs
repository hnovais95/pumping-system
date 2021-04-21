using PumpingSystem.Shared;

namespace PumpingSystem.Domain.Repository
{
    public interface ILocalRepository
    {
        ProcessChart ProcessChart { get; set; }
        Pump Pump { get; set; }
        WaterTank[] Tanks { get; set; }
    }
}
