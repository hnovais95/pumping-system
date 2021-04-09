using System;
using System.Windows.Forms;
using System.IO.Ports;
using PumpingSystem.Driver;
using PumpingSystem.View;
using PumpingSystem.Server;
using PumpingSystem.Server.Repository;

namespace PumpingSystem
{
    public static class Program
    {
        private static IUpdatableForm<Form> _View;
        private static RepositoryService _RepositoryService;
        public static RTDB RTDB;
        public static UartDriver UartDriver;
        public static UartService UartService;
        public static ViewService ViewService;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            RTDB = new RTDB();

            UartService = new UartService();
            UartDriver = new UartDriver(new SerialPort());

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _View = new frmMain();
            _RepositoryService = new RepositoryService(new ProcessChartDao());
            ViewService = new ViewService(_View, _RepositoryService);
            ViewService.InitializeDataPublisherTimer(2000);
            ViewService.InitializeProcessChartUpdaterTimer(5000);

            Application.Run((Form)_View);
        }
    }
}
