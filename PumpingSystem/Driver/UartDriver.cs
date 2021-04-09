using System;
using System.IO.Ports;
using System.Reflection;
using System.ComponentModel;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Server;

namespace PumpingSystem.Driver
{
    public class UartDriver
    {
        private SerialPort _SerialPort;

        public UartDriver(SerialPort serialPort)
        {
            _SerialPort = serialPort;
            _SerialPort.PortName = "COM1";
            _SerialPort.WriteTimeout = 1000;
            _SerialPort.ReadTimeout = 1000;
            _SerialPort.BaudRate = 9600;
            _SerialPort.Parity = Parity.None;
            _SerialPort.StopBits = StopBits.One;
            _SerialPort.DataBits = 8;
            _SerialPort.Handshake = Handshake.None;
            _SerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            _SerialPort.Open();
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort serialPort = (SerialPort)sender;
                string dataReceived = serialPort.ReadExisting();

                string[] buffer = dataReceived?.Split(';');
                if (buffer != null && buffer.Length > 0)
                {
                    UartTelegram telegram = new UartTelegram(buffer);
                    IMsgUart msg = MsgUartFactory.CreateMsg(telegram.Header.ID);

                    PropertyInfo[] propsInfo = msg.GetType().GetProperties();
                    if ((propsInfo != null) && (propsInfo.Length == telegram.Data.Length))
                    {
                        for (int i = 0; i < propsInfo.Length; i++)
                        {
                            var converter = TypeDescriptor.GetConverter(propsInfo[i].PropertyType);
                            var result = converter.ConvertFrom(telegram.Data[i]);
                            propsInfo[i].SetValue(msg, result);
                        }

                        Program.UartService.TreatMessage(msg);
                    }
                }
            }
            catch (Exception exc)
            {
            }
        }

        public void SendDataHandler(IMsgUart msg)
        {
            try
            {
                UartHeader header = null;
                string[] data = null;

                PropertyInfo[] propsInfo = msg.GetType().GetProperties();
                if ((propsInfo != null) && (propsInfo.Length > 0))
                {
                    header = new UartHeader
                    {
                        Size = propsInfo.Length,
                        ID = msg.GetID()
                    };

                    data = new string[propsInfo.Length];
                    for (int i = 0; i < propsInfo.Length; i++)
                    {
                        data[i] = propsInfo[i].GetValue(msg, null).ToString();
                    }
                }

                if (header != null)
                {
                    UartTelegram telegram = new UartTelegram(header, data);
                    _SerialPort.WriteLine(string.Join(";", telegram.Buffer));
                }
                else
                {
                    throw new Exception("Impossível criar telegrama sem cabeçalho.");
                }
            }
            catch (Exception exc)
            {
            }
        }
    }
}
