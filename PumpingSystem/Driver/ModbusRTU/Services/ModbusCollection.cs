using IndustrialNetworks.ModbusRTU.Cores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IndustrialNetworks.ModbusRTU.Services
{
    public class ModbusCollection
    {
        private static List<Memory> _Registers;

        public static void InitializeRegisters()
        {
            if (_Registers == null)
            {
                _Registers = new List<Memory>();
                _Registers.Add(new Memory() { Address = 40001 });
                _Registers.Add(new Memory() { Address = 40002 });
                _Registers.Add(new Memory() { Address = 40003 });
                _Registers.Add(new Memory() { Address = 40004 });
                _Registers.Add(new Memory() { Address = 40005 });
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
