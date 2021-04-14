using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Timers;
using IndustrialNetworks.ModbusRTU.Cores;

namespace IndustrialNetworks.ModbusRTU.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class ReadHoldingRegistersService : IReadHoldingRegistersService
    {

        private IModbusRTUDataChanged eventDataChanged = null;
        private Timer timerUpdate = null;

        public ReadHoldingRegistersService()
        {
            try
            {
                if(timerUpdate == null)
                {
                    timerUpdate = new Timer();
                    timerUpdate.Elapsed += timerUpdate_Elapsed;
                    timerUpdate.Interval = 100;
                    timerUpdate.Start();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void timerUpdate_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if(eventDataChanged != null)
                {
                    eventDataChanged.DataChanged(ModbusCollection.Registers);
                }
            }
            catch (Exception)
            {

            }
        }


        public void Connection()
        {
            try
            {
                eventDataChanged = OperationContext.Current.GetCallbackChannel<IModbusRTUDataChanged>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Disconnection()
        {
            try
            {
                eventDataChanged = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
