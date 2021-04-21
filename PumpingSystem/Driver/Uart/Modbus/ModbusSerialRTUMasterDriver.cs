using System;
using System.Reflection;
using System.IO.Ports;
using System.Threading.Tasks;
using Modbus.Device;
using PumpingSystem.Messages.Uart;

namespace PumpingSystem.Driver.Uart.Modbus
{
    public class ModbusSerialRTUMasterDriver
    {
        private SerialPort _SerialPort;
        private const byte _SlaveId = 1;
        private const ushort _StartAddress = 0;
        private ushort _NumberOfPoints = 8;
        private ModbusSerialMaster _Master;
        private ushort[] _Registers;

        private enum RegisterTypeByPosition
        {
            PumpStatus = 0,
            OperationMode = 1,
            MinWaterTankLevel1 = 2,
            MinWaterTankLevel2 = 3,
            WaterTankLevel1 = 4,
            WaterTankLevel2 = 5
        }

        public ModbusSerialRTUMasterDriver(SerialPort serialPort)
        {
            _SerialPort = serialPort;
            _Master = ModbusSerialMaster.CreateRtu(_SerialPort);
            _Registers = new ushort[_NumberOfPoints];
        }

        public void Initialize()
        {
            _SerialPort.PortName = "COM2";
            _SerialPort.BaudRate = 9600;
            _SerialPort.DataBits = 8;
            _SerialPort.Parity = Parity.None;
            _SerialPort.StopBits = StopBits.One;
            _SerialPort.Open();
            Loop();            
        }

        private void Loop()
        {
            Task.Run(async delegate
            {
                try
                {
                    while (true)
                    {
                        ushort[] registers = _Master.ReadHoldingRegisters(_SlaveId, _StartAddress, _NumberOfPoints);

                        for (int i = 0; i < registers.Length; i++)
                        {
                            if (registers[i] != _Registers[i])
                            {
                                IMsgUart msg = CreateMsg((RegisterTypeByPosition)i, registers);
                                if (msg != null)
                                {
                                    Program.UartService.TreatMessage(msg);
                                }
                            }
                        }

                        _Registers = registers;

                        await Task.Delay(1000);
                    }
                }
                catch (Exception e)
                {

                }
            });
        }

        public void WriteRegisters(IMsgUart msg)
        {
            try
            {
                short startAddress = GetStartAddress(msg);
                if (startAddress != -1)
                {
                    var propsInfo = msg.GetType().GetProperties();
                    if ((propsInfo != null) && (propsInfo.Length > 0))
                    {
                        for (int i = 0; i < propsInfo.Length; i++)
                        {
                            ushort data = Convert.ToUInt16(propsInfo[i].GetValue(msg, null));
                            _Master.WriteSingleRegister(_SlaveId, (ushort)(startAddress + i), data);
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
        }

        private IMsgUart CreateMsg(RegisterTypeByPosition registerType, ushort[] registers)
        {
            IMsgUart msg = null;
            switch (registerType)
            {
                case RegisterTypeByPosition.PumpStatus:
                case RegisterTypeByPosition.OperationMode:

                    int pumpStauts = registers[(int)RegisterTypeByPosition.PumpStatus];
                    int operationMode = registers[(int)RegisterTypeByPosition.OperationMode];                    
                    msg = new MsgTelegram100(pumpStauts, operationMode);

                    break;
                case RegisterTypeByPosition.MinWaterTankLevel1:
                case RegisterTypeByPosition.MinWaterTankLevel2:

                    int minWaterTankLevel1 = registers[(int)RegisterTypeByPosition.MinWaterTankLevel1];
                    int minWaterTankLevel2 = registers[(int)RegisterTypeByPosition.MinWaterTankLevel2];
                    msg = new MsgTelegram101(minWaterTankLevel1, minWaterTankLevel2);

                    break;
                case RegisterTypeByPosition.WaterTankLevel1:
                case RegisterTypeByPosition.WaterTankLevel2:

                    int waterTankLevel1 = registers[(int)RegisterTypeByPosition.WaterTankLevel1];
                    int waterTankLevel2 = registers[(int)RegisterTypeByPosition.WaterTankLevel2];
                    msg = new MsgTelegram102(waterTankLevel1, waterTankLevel2);

                    break;
            }
            return msg;
        }
        
        private short GetStartAddress(IMsgUart msg)
        {
            switch (msg.GetID())
            {
                case 200:
                    return 0;
                case 201:
                    return 2;
                default:
                    return -1;
            }
        }
    }
}
