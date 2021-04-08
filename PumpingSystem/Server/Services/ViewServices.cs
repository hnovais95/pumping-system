using System;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using PumpingSystem.Messages.View;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Common;
using PumpingSystem.View;

namespace PumpingSystem.Server
{
    public class ViewServices
    {
        private System.Timers.Timer _DataPublisherTimer;
        private System.Timers.Timer _ProcessChartUpdaterTimer;
        private IUpdatableForm<Form> _View;

        public ViewServices(IUpdatableForm<Form> view)
        {
            _View = view;
        }

        public void InitializeDataPublisherTimer(int interval)
        {
            _DataPublisherTimer = new System.Timers.Timer(interval);
            _DataPublisherTimer.Elapsed += PublishData;
            _DataPublisherTimer.AutoReset = true;
            _DataPublisherTimer.Enabled = true;
        }

        public void InitializeProcessChartUpdaterTimer(int interval)
        {
            _ProcessChartUpdaterTimer = new System.Timers.Timer(interval);
            _ProcessChartUpdaterTimer.Elapsed += UpdateProcessChart;
            _ProcessChartUpdaterTimer.AutoReset = true;
            _ProcessChartUpdaterTimer.Enabled = true;
        }

        private void UpdateProcessChart(Object source, ElapsedEventArgs e)
        {
            RTDB rtdb = Program.RTDB;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Monitor.TryEnter(rtdb);

                    DataProcessChart data = new DataProcessChart();
                    for (int i = 0; i < rtdb.Tanks.Length; i++)
                    {
                        data.Level[i] = rtdb.Tanks[i].Level;
                    }
                    data.OperationMode = (int)rtdb.Pump.OperationMode;
                    data.PumpStatus = (int)rtdb.Pump.Status;

                    rtdb.ProcessChart.Add(data);
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
                        send |= waterTank.Alterada;
                        waterTank.Alterada = false;
                    }

                    if (send)
                    {
                        PublishDataWaterTank(msgsDataWaterTank);
                    }

                    if (rtdb.Pump.Alterada)
                    {
                        MsgDataPump msg = new MsgDataPump(rtdb.Pump.Status);
                        PublishDataPump(msg);
                        rtdb.Pump.Alterada = false;
                    }

                    if (rtdb.Pump.Alterada)
                    {
                        MsgOperationMode msg = new MsgOperationMode(rtdb.Pump.OperationMode);
                        PublishOperationMode(msg);
                        rtdb.Pump.Alterada = false;
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

        private void PublishOperationMode(MsgOperationMode msg)
        {
            _View.UpdateOperationMode(msg);
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

                    Program.InterfaceUart.SendMessage(new MsgTelegram200((int)msg.StatusPump));
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

        public void SendOperationMode(MsgOperationMode msg)
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

                    Program.InterfaceUart.SendMessage(new MsgTelegram201((int)msg.OperationMode));
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
