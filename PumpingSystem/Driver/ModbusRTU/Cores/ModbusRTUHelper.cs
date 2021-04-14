using IndustrialNetworks.ModbusRTU.DataTypes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace IndustrialNetworks.ModbusRTU.Cores
{
    public class ModbusRTUHelper
    {
        private static ModbusRTUMaster objModbusRTUMaster = null;

        private static List<Memory> _Registers;

        public static void Start()
        {
            try
            {
                byte slaveAddress = 1;
                byte function = 3;
                ushort startAddress = _Registers[0].Address;
                ushort numberOfPoints = (ushort)_Registers.Count;

                _Registers.Sort();
                if (_Registers == null) throw new InvalidOperationException("The list registers is empty");

                objModbusRTUMaster = (objModbusRTUMaster == null) ? objModbusRTUMaster = new ModbusRTUMaster("COM1", 9600, Parity.None, 8, StopBits.One) : objModbusRTUMaster;
                objModbusRTUMaster.Connect();

                ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
                {
                    while (true)
                    {
                        ushort[] result = objModbusRTUMaster.ReadHoldingRegisters(slaveAddress, startAddress, function, numberOfPoints);
                        if (result == null) continue;             
                        if (result.Length >= _Registers.Count)
                        {
                            for (int i = 0; i < _Registers.Count; i++)
                            {
                                _Registers[i].Value = string.Format("{0}", result[i]);
                            }
                        }
                        Thread.Sleep(100); // Delay 100ms
                    }

                }));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void Stop()
        {
            try
            {
                objModbusRTUMaster.Disconnect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Memory> Registers
        {
            get
            {
                return _Registers;
            }
            set
            {
                _Registers = value;
            }
        }
    }
}
