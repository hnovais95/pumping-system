using System;
using System.Windows.Forms;
using System.IO.Ports;
using PumpingSystem.Driver;
using PumpingSystem.Pumping;
using PumpingSystem.View;
using PumpingSystem.Databases;

namespace PumpingSystem
{
    public static class Program
    {
        private static IUpdateForm<Form> _View;
        private static string ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\Databases\\DbPumpingSystem.mdf;Integrated Security = True";
        public static RTDB RTDB;
        public static LocalDb LocalDb;
        public static DriverUart DriverUart;
        public static InterfaceUart InterfaceUart;
        public static InterfaceView InterfaceView;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RTDB = new RTDB();
            LocalDb = new LocalDb(ConnectionString);
            InterfaceUart = new InterfaceUart();
            DriverUart = new DriverUart(new SerialPort());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _View = new frmMain();
            InterfaceView = new InterfaceView(_View);
            InterfaceView.InitializeTimer(2000);

            Application.Run((Form)_View);
        }
    }
}
