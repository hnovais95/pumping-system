using System;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using PumpingSystem.Messages.View;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Common;
using PumpingSystem.View;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class ViewService
    {
        private System.Timers.Timer _DataPublisherTimer;
        private System.Timers.Timer _ProcessChartUpdaterTimer;
        private IUpdatableForm<Form> _View;
        private RepositoryService _RepositoryService;

        public ViewService(IUpdatableForm<Form> view, RepositoryService repositoryService)
        {
            _View = view;
            _RepositoryService = repositoryService;
        }

        public void InitializeDataPublisherTimer(int interval)
        {
            _DataPublisherTimer = new System.Timers.Timer(interval);
            _DataPublisherTimer.Elapsed += PublishData;
            _DataPublisherTimer.AutoReset = true;
            _DataPublisherTimer.Enabled = false;
        }

        public void InitializeProcessChartUpdaterTimer(int interval)
        {
            _ProcessChartUpdaterTimer = new System.Timers.Timer(interval);
            _ProcessChartUpdaterTimer.Elapsed += UpdateProcessChart;
            _ProcessChartUpdaterTimer.AutoReset = true;
            _ProcessChartUpdaterTimer.Enabled = false;
        }

        public void UpdateProcessChart(Object source, ElapsedEventArgs e)
        {
            RTDB rtdb = Program.RTDB;
            //Task.Factory.StartNew(() =>
            //{
                try
                {
                    //Monitor.TryEnter(rtdb);

                    DataProcessChart data = new DataProcessChart();
                    for (int i = 0; i < rtdb.Tanks.Length; i++)
                    {
                        data.Level[i] = rtdb.Tanks[i].Level;
                    }
                    data.OperationMode = (int)rtdb.Pump.OperationMode;
                    data.PumpStatus = (int)rtdb.Pump.Status;

                    rtdb.ProcessChart.Add(data);
                    //_RepositoryService.InsertProcessChart(rtdb.ProcessChart);

                    /*var strStartDate = "09/04/2021 17:00:00";
                    var strEndDate = "09/04/2021 18:00:00";
                    DateTime startDate = Convert.ToDateTime(strStartDate);
                    DateTime endDate = Convert.ToDateTime(strEndDate);

                    _RepositoryService.GetProcessChartByPeriod(startDate, endDate);*/
                }
                catch (Exception exc)
                {

                }
                finally
                {
              //      Monitor.Exit(rtdb);
                }
            //});
        }

        private void PublishData(Object source, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                RTDB rtdb = Program.RTDB;

                try
                {
                    Monitor.TryEnter(rtdb);

                    bool send = false;

                    MsgDataWaterTank[] msgsDataWaterTank = new MsgDataWaterTank[2];
                    for (int i = 0; i < rtdb.Tanks.Length; i++)
                    {
                        WaterTank waterTank = rtdb.Tanks[i];
                        msgsDataWaterTank[i] = new MsgDataWaterTank(waterTank.Level, waterTank.MinLevel);
                        send |= waterTank.Changed;
                        waterTank.Changed = false;
                    }

                    if (send)
                    {
                        PublishDataWaterTank(msgsDataWaterTank);
                    }

                    if (rtdb.Pump.Changed)
                    {
                        MsgDataPump msg = new MsgDataPump(rtdb.Pump.Status, rtdb.Pump.OperationMode);
                        PublishDataPump(msg);
                        rtdb.Pump.Changed = false;
                    }
                }
                catch (Exception exc)
                {

                }
                finally
                {
                    Monitor.Exit(rtdb);
                }
            });
        }

        private void PublishDataWaterTank(MsgDataWaterTank[] msgs)
        {
            _View.UpdateWaterTanks(msgs);
        }

        private void PublishDataPump(MsgDataPump msg)
        {
            _View.UpdatePump(msg);
        }

        public void SendPumpStatus(MsgDataPump msg)
        {
            RTDB rtdb = Program.RTDB;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Monitor.TryEnter(rtdb);

                    if (msg.StatusPump == EnumPumpStatus.On)
                        Program.RTDB.Pump.TurnOnPump();
                    else
                        Program.RTDB.Pump.TurnOffPump();

                    Program.UartService.SendMessage(new MsgTelegram200((int)msg.StatusPump));
                }
                catch (Exception e)
                {
                }
                finally
                {
                    Monitor.Exit(rtdb);
                }
            });
        }

        public void SendOperationMode(MsgDataPump msg)
        {
            RTDB rtdb = Program.RTDB;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Monitor.TryEnter(rtdb);

                    if (msg.OperationMode == EnumOperationMode.Automatic)
                        Program.RTDB.Pump.OperationMode = EnumOperationMode.Automatic;
                    else
                        Program.RTDB.Pump.OperationMode = EnumOperationMode.Manual;

                    Program.UartService.SendMessage(new MsgTelegram201((int)msg.OperationMode));
                }
                catch (Exception e)
                {
                }
                finally
                {
                    Monitor.Exit(rtdb);
                }
            });
        }
    }
}
