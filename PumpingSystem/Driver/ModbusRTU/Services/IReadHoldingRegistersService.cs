using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IndustrialNetworks.ModbusRTU.Services
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IModbusRTUDataChanged))]
    public interface IReadHoldingRegistersService
    {
        [OperationContract(IsOneWay = true)]
        void Connection();

        [OperationContract(IsOneWay = true)]
        void Disconnection();
    }
}
