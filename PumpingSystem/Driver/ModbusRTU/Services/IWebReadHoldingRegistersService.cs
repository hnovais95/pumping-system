using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace IndustrialNetworks.ModbusRTU.Services
{
    [ServiceContract]
    public interface IWebReadHoldingRegistersService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "ReadHoldingRegisters", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Cores.Memory> ReadHoldingRegisters();
    }
}
