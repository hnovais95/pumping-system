using System;
using System.Windows.Forms;
using System.IO.Ports;
using PumpingSystem.Driver.Uart;
using PumpingSystem.Driver.Uart.Modbus;
using PumpingSystem.View;
using PumpingSystem.Server;
using PumpingSystem.Server.Repository;

namespace PumpingSystem
{
    public static class Program
    {
        //public static UartDriver UartDriver;
        public static ModbusSerialRTUMasterDriver ModbusSerialRTUMasterDriver;
        public static UartService UartService;
        private static RepositoryService _RepositoryService;
        public static ApplicationService ApplicationService;
        public static frmMain FrmMain;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //UartDriver = new UartDriver(new SerialPort());
            ModbusSerialRTUMasterDriver = new ModbusSerialRTUMasterDriver(new SerialPort());
            ModbusSerialRTUMasterDriver.Initialize();
            UartService = new UartService();
            _RepositoryService = new RepositoryService(new ProcessChartRepository(), new AuthenticationRepository());
            FrmMain = new frmMain();
            ApplicationService = new ApplicationService(_RepositoryService);
            ApplicationService.InitializeDataPublishing(1000);
            ApplicationService.InitializeProcessChartUpdate(1000);
            ApplicationService.InitializeProcessChartStoragerTimer(30000);

            Application.Run((Form)FrmMain);
        }
    }
}
