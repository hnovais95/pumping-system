using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace IndustrialNetworks.ModbusRTU.Cores
{
    [DataContract]
    public class Memory : IComparable<Memory>, INotifyPropertyChanged   
    {
        private ushort _Address;
        private string _Value;

        [DataMember]
        public ushort Address
        {
            get
            {
                return _Address;
            }

            set
            {
                _Address = value;
            }
        }

        [DataMember]
        public string Value
        {
            get { return _Value; }
            set
            {
                if (_Value == value.ToString()) return;
                _Value = value;
                OnPropertyChanged("Value");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(newName));
            }
        }

        public int CompareTo(Memory obj)
        {
            return this.Address.CompareTo(obj.Address);
        }
    }
}
