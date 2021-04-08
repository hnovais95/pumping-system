using System;
using System.Windows.Forms;
using System.IO.Ports;
using PumpingSystem.Driver;
using PumpingSystem.View;
using PumpingSystem.Databases;

namespace PumpingSystem.Server
{
    public static class Program
    {
        private static IUpdatableForm<Form> _View;
        private static string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Databases\\DbPumpingSystem.mdf;Integrated Security = True";
        public static RTDB RTDB;
        public static LocalDb LocalDb;
        public static UartDriver DriverUart;
        public static UartInterface InterfaceUart;
        public static ViewInterface InterfaceView;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RTDB = new RTDB();
            LocalDb = new LocalDb(ConnectionString);
            InterfaceUart = new UartInterface();
            DriverUart = new UartDriver(new SerialPort());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _View = new frmMain();
            InterfaceView = new ViewInterface(_View);
            InterfaceView.InitializeDataPublisherTimer(2000);

            Application.Run((Form)_View);
        }
    }
}
