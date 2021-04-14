using System;
using System.Collections.Generic;
using IndustrialNetworks.ModbusRTU.Cores;

namespace IndustrialNetworks.ModbusRTU.Services
{
    public class WebReadHoldingRegistersService : IWebReadHoldingRegistersService
    {
        public List<Memory> ReadHoldingRegisters()
        {
            try
            {
                return ModbusCollection.Registers;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
