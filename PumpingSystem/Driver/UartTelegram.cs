using System;
using System.Reflection;
using PumpingSystem.Messages.Uart;
using System.ComponentModel;

namespace PumpingSystem.Driver
{
    public class UartTelegram
    {
        private UartHeader _Header;
        private string[] _Data;
        private string[] _Buffer;

        public UartTelegram(string[] buffer)
        {
            _Buffer = buffer;
            _Header = null;
            _Data = null;
        }

        public UartTelegram(UartHeader header, string[] data)
        {
            if (header == null)
                throw new Exception("Impossível montar telegrama com cabeçalho nulo");
            
            _Header = header;
            _Data = data;
            _Buffer = null;
        }

        public UartHeader Header
        {
            get
            {
                if (_Header == null)
                {
                    int headerSize = typeof(UartHeader).GetProperties().Length;
                    if (_Buffer != null && _Buffer.Length >= headerSize)
                    {
                        _Header = new UartHeader
                        {
                            Size = int.Parse(_Buffer[0]),
                            ID = int.Parse(_Buffer[1])
                        };
                    }
                    else
                        throw new Exception("Impossível ler cabeçalho do telegrama, buffer inválido");
                }
                return _Header;
            }
        }

        public string[] Data
        {
            get
            {
                if (_Data == null)
                {
                    int headerSize = typeof(UartHeader).GetProperties().Length;
                    if ((_Buffer != null) && (_Buffer.Length > headerSize))
                    {
                        _Data = new string[_Buffer.Length - headerSize];
                        Array.Copy(_Buffer, headerSize, _Data, 0, _Data.Length);
                    }
                    else
                        throw new Exception("Impossível dados do telegrama, buffer inválido");
                }
                return _Data;
            }
        }

        public string[] Buffer
        {
            get
            {
                if (_Buffer == null)
                {
                    if (_Header != null)
                    {
                        int headerSize = typeof(UartHeader).GetProperties().Length;
                        int dataSize = 0;
                        if (_Data != null)
                        {
                            dataSize = _Data.Length;
                        }
                        _Buffer = new string[headerSize + dataSize];
                        string[] headerData = { _Header.Size.ToString(), _Header.ID.ToString() };
                        Array.Copy(headerData, _Buffer, headerData.Length);
                        if (dataSize != 0)
                        {
                            Array.Copy(_Data, 0, _Buffer, headerData.Length, _Data.Length);
                        }
                    }
                    else
                        throw new Exception("Impossível criar buffer de telegrama sem cabeçalho");
                }
                return _Buffer;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} - {1}", Header, (_Data != null) ? _Data.ToString() : "NI");
        }
    }
}
