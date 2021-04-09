using System;
using System.Windows.Forms;
using PumpingSystem.Messages.View;

namespace PumpingSystem.View
{
    public interface IUpdatableForm <T> where T : Form
    {
        void UpdateWaterTanks(MsgDataWaterTank[] msgs);
        void UpdatePump(MsgDataPump msg);
    }
}
