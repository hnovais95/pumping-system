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
        private const ushort _StartAddress = 40001;
        private ushort _NumberOfPoints = 8;
        private ModbusSerialMaster _Master;
        private ushort[] _Registers;

        private enum EnumRegisterType
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
                                IMsgUart msg = CreateMsg((EnumRegisterType)i, registers);
                                if (msg != null)
                                {
                                    Program.UartService.TreatMessage(msg);
                                }
                            }
                        }

                        _Registers = registers;

                        await Task.Delay(2000);
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
                ushort increment = 0;
                ushort[] data = null;

                switch (msg.GetID())
                {
                    case 200:

                        if (GetIncrementAddress(msg) != -1)
                        {
                            increment = (ushort)GetIncrementAddress(msg);

                            MsgTelegram200 tel = (MsgTelegram200)msg;

                            var propsInfo = tel.GetType().GetProperties();
                            if ((propsInfo != null) && (propsInfo.Length > 0))
                            {
                                data = new ushort[propsInfo.Length];
                                for (int i = 0; i < propsInfo.Length; i++)
                                {
                                    data[i] = Convert.ToUInt16(propsInfo[i].GetValue(msg, null));
                                }
                            }

                        }

                        break;
                }

                if ((data != null) && (data.Length > 0))
                {
                    _Master.WriteMultipleRegisters(_SlaveId, (ushort)(_StartAddress + increment), data/*_Registers*/);
                }
            }
            catch (Exception e)
            {

            }
        }

        private IMsgUart CreateMsg(EnumRegisterType registerType, ushort[] registers)
        {
            IMsgUart msg = null;
            switch (registerType)
            {
                case EnumRegisterType.PumpStatus:
                case EnumRegisterType.OperationMode:

                    int pumpStauts = registers[(int)EnumRegisterType.PumpStatus];
                    int operationMode = registers[(int)EnumRegisterType.OperationMode];                    
                    msg = new MsgTelegram100(pumpStauts, operationMode);

                    break;
                case EnumRegisterType.MinWaterTankLevel1:
                case EnumRegisterType.MinWaterTankLevel2:

                    int minWaterTankLevel1 = registers[(int)EnumRegisterType.MinWaterTankLevel1];
                    int minWaterTankLevel2 = registers[(int)EnumRegisterType.MinWaterTankLevel2];
                    msg = new MsgTelegram102(minWaterTankLevel1, minWaterTankLevel2);

                    break;
                case EnumRegisterType.WaterTankLevel1:
                case EnumRegisterType.WaterTankLevel2:

                    int waterTankLevel1 = registers[(int)EnumRegisterType.WaterTankLevel1];
                    int waterTankLevel2 = registers[(int)EnumRegisterType.WaterTankLevel2];
                    msg = new MsgTelegram101(waterTankLevel1, waterTankLevel2);

                    break;
            }
            return msg;
        }
        
        private short GetIncrementAddress(IMsgUart msg)
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
