using IndustrialNetworks.ModbusRTU.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace IndustrialNetworks.ModbusRTU.Services
{
    public interface IModbusRTUDataChanged
    {
        [OperationContract(IsOneWay = true)]
        void DataChanged(List<Memory> registers);
    }
}
