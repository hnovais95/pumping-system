using IndustrialNetworks.ModbusRTU.DataTypes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IndustrialNetworks.ModbusRTU.Cores
{
    public class ModbusRTUMaster
    {
        private const int READ_BUFFER_SIZE = 2048; // 2KB.

        private const int WRITE_BUFFER_SIZE = 2048; // 2KB.

        private byte[] bufferReceiver = null;
        private byte[] bufferSender = null;

        private SerialPort serialPort1 = null;

        public ModbusRTUMaster(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            serialPort1 = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        }


        /// <summary>
        /// Connect to device
        /// </summary>
        public void Connect()
        {
            try
            {
                if(serialPort1 == null) serialPort1 = new SerialPort("COM1", 9600, Parity.None, 8, StopBits.One);
                serialPort1.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Disconnect with device
        /// </summary>
        public void Disconnect()
        {
            try
            {
                if(serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Write data
        /// </summary>
        /// <param name="frame">Frame communication</param>
        public void Write(byte[] frame)
        {
            serialPort1.Write(frame, 0, frame.Length);
        }

        /// <summary>
        /// Read Data
        /// </summary>
        /// <returns>byte array</returns>
        public byte[] Read()
        {
            if (serialPort1.BytesToRead >= 5)
            {
                this.bufferReceiver = new byte[this.serialPort1.BytesToRead];
                serialPort1.Read(this.bufferReceiver, 0, serialPort1.BytesToRead);
            }
            return this.bufferReceiver;
        }

        /// <summary>
        /// Function 03 (03hex) Read Holding Registers
        /// Read the binary contents of holding registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>
        public ushort[] ReadHoldingRegisters(byte slaveAddress, ushort startAddress, byte function, ushort numberOfPoints)
        {
            ushort[] result = null;
            if (serialPort1.IsOpen)
            {
                byte[] frame = this.ReadHoldingRegistersMsg(slaveAddress, startAddress, function, numberOfPoints);

                serialPort1.Write(frame, 0, frame.Length);
                Thread.Sleep(100); // Delay 100ms
                if (serialPort1.BytesToRead >= 5)
                {
                    byte[] bufferReceiver = new byte[this.serialPort1.BytesToRead];
                    serialPort1.Read(bufferReceiver, 0, serialPort1.BytesToRead);
                    serialPort1.DiscardInBuffer();

                    // Process data.
                    byte[] data = new byte[bufferReceiver.Length - 5];
                    Array.Copy(bufferReceiver, 3, data, 0, data.Length);
                    result = Word.ConvertByteArrayToWordArray(data);
                }
            }
            return result;
        }

        /// <summary>
        /// Function 03 (03hex) Read Holding Registers
        /// Read the binary contents of holding registers in the slave.
        /// </summary>
        /// <param name="slaveAddress">Slave Address</param>
        /// <param name="startAddress">Starting Address</param>
        /// <param name="function">Function</param>
        /// <param name="numberOfPoints">Quantity of inputs</param>
        /// <returns>Byte Array</returns>
        private byte[] ReadHoldingRegistersMsg(byte slaveAddress, ushort startAddress, byte function, ushort numberOfPoints)
        {
            byte[] frame = new byte[8];
            frame[0] = slaveAddress;			    // Slave Address
            frame[1] = function;				    // Function             
            frame[2] = (byte)(startAddress >> 8);	// Starting Address High
            frame[3] = (byte)startAddress;		    // Starting Address Low            
            frame[4] = (byte)(numberOfPoints >> 8);	// Quantity of Registers High
            frame[5] = (byte)numberOfPoints;		// Quantity of Registers Low
            byte[] crc = this.CalculateCRC(frame);  // Calculate CRC.
            frame[frame.Length - 2] = crc[0];       // Error Check Low
            frame[frame.Length - 1] = crc[1];       // Error Check High
            return frame;
        }

        /// <summary>
        /// CRC Calculation 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private byte[] CalculateCRC(byte[] data)
        {
            ushort CRCFull = 0xFFFF; // Set the 16-bit register (CRC register) = FFFFH.
            char CRCLSB;
            byte[] CRC = new byte[2];
            for (int i = 0; i < (data.Length) - 2; i++)
            {
                CRCFull = (ushort)(CRCFull ^ data[i]); // 

                for (int j = 0; j < 8; j++)
                {
                    CRCLSB = (char)(CRCFull & 0x0001);
                    CRCFull = (ushort)((CRCFull >> 1) & 0x7FFF);

                    if (CRCLSB == 1)
                        CRCFull = (ushort)(CRCFull ^ 0xA001);
                }
            }
            CRC[1] = (byte)((CRCFull >> 8) & 0xFF);
            CRC[0] = (byte)(CRCFull & 0xFF);
            return CRC;
        }

        /// <summary>
        /// Check validate(Modbus Exception code).
        /// </summary>
        /// <param name="messageReceived">frame</param>
        public void CheckValidate(byte[] messageReceived)
        {
            try
            {
                switch (messageReceived[1])
                {

                    case 129: // Hex: 81                     
                    case 130: // Hex: 82 
                    case 131: // Hex: 83 
                    case 132: // Hex: 83 
                    case 133: // Hex: 84 
                    case 134: // Hex: 86 
                    case 143: // Hex: 8F 
                    case 144: // Hex: 90
                        switch (messageReceived[2])
                        {
                            case 1:
                                throw new Exception("01/0x01: Illegal Function.");
                            case 2:
                                throw new Exception("02/0x02: Illegal Data Address.");
                            case 3:
                                throw new Exception("03/0x03: Illegal Data Value.");
                            case 4:
                                throw new Exception("04/0x04: Failure In Associated Device.");
                            case 5:
                                throw new Exception("05/0x05: Acknowledge.");
                            case 6:
                                throw new Exception("06/0x06: Slave Device Busy.");
                            case 7:
                                throw new Exception("07/0x07: NAK – Negative Acknowledgements.");
                            case 8:
                                throw new Exception("08/0x08: Memory Parity Error.");
                            case 10:
                                throw new Exception("10/0x0A: Gateway Path Unavailable.");
                            case 11:
                                throw new Exception("11/0x0B: Gateway Target Device Failed to respond.");
                            default:
                                break;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
