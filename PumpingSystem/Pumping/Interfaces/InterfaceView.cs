using System;
using System.Windows.Forms;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using PumpingSystem.Messages.View;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Common;
using PumpingSystem.View;

namespace PumpingSystem.Pumping
{
    public class InterfaceView
    {
        private System.Timers.Timer _Timer;
        private IUpdateForm<Form> _View;

        public InterfaceView(IUpdateForm<Form> view)
        {
            _View = view;
        }

        public void InitializeTimer(int interval)
        {
            _Timer = new System.Timers.Timer(interval);
            _Timer.Elapsed += OnTimedEvent;
            _Timer.AutoReset = true;
            _Timer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                RTDB rtdb = Program.RTDB;

                try
                {
                    Monitor.TryEnter(rtdb);

                    bool send = false;

                    MsgDataWaterTank[] msgsDataWaterTank = new MsgDataWaterTank[2];
                    for (int i = 0; i < rtdb.Pumping.Tanks.Length; i++)
                    {
                        WaterTank waterTank = rtdb.Pumping.Tanks[i];
                        msgsDataWaterTank[i] = new MsgDataWaterTank(waterTank.Level, waterTank.MinLevel);
                        send |= waterTank.Alterada;
                        waterTank.Alterada = false;
                    }

                    if (send)
                    {
                        PublishDataWaterTank(msgsDataWaterTank);
                    }

                    if (rtdb.Pumping.Pump.Alterada)
                    {
                        MsgDataPump msg = new MsgDataPump(rtdb.Pumping.Pump.Status);
                        PublishDataPump(msg);
                        rtdb.Pumping.Pump.Alterada = false;
                    }

                    if (rtdb.Pumping.Alterada)
                    {
                        MsgOperationMode msg = new MsgOperationMode(rtdb.Pumping.OperationMode);
                        PublishOperationMode(msg);
                        rtdb.Pumping.Alterada = false;
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
                        Program.RTDB.Pumping.TurnOnPump();
                    else
                        Program.RTDB.Pumping.TurnOffPump();

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
                        Program.RTDB.Pumping.OperationMode = EnumOperationMode.Automatic;
                    else
                        Program.RTDB.Pumping.OperationMode = EnumOperationMode.Manual;

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
