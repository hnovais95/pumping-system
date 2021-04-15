using System;
using System.Windows.Forms;
using PumpingSystem.Messages.View;

namespace PumpingSystem.View
{
    public interface IUpdatableForm <T> where T : Form
    {
        void UpdateWaterTanks(MsgWaterTankData[] msgs);
        void UpdatePump(MsgPumpData msg);
        void UpdateChart(MsgChartData msg);
    }
}
