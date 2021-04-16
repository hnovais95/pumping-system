using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using PumpingSystem.Messages.View;
using PumpingSystem.Messages.Uart;
using PumpingSystem.Common;
using PumpingSystem.Server.Repository;

namespace PumpingSystem.Server
{
    public class ApplicationService
    {
        private Timer _ProcessChartStoragerTimer;
        private RepositoryService _RepositoryService;

        public ApplicationService(RepositoryService repositoryService)
        {
            _RepositoryService = repositoryService;
        }

        public void InitializeDataPublishing(int interval)
        {
            DataPublishingLoop(interval);
        }

        private void DataPublishingLoop(int interval)
        {
            Task.Run(async delegate
            {
                LocalRepository localRepository = LocalRepository.GetInstance();

                try
                {
                    while (true)
                    {
                        bool send = false;

                        MsgWaterTankData[] msgsDataWaterTank = new MsgWaterTankData[2];
                        for (int i = 0; i < localRepository.Tanks.Length; i++)
                        {
                            WaterTank waterTank = localRepository.Tanks[i];
                            msgsDataWaterTank[i] = new MsgWaterTankData(waterTank.Level, waterTank.MinLevel);
                            send |= waterTank.Changed;
                            waterTank.Changed = false;
                        }

                        if (send)
                        {
                            PublishDataWaterTank(msgsDataWaterTank);
                        }

                        if (localRepository.Pump.Changed)
                        {
                            MsgPumpData msg = new MsgPumpData(localRepository.Pump.Status, localRepository.Pump.OperationMode);
                            PublishDataPump(msg);
                            localRepository.Pump.Changed = false;
                        }

                        for (int i = 0; i < localRepository.ProcessChart.Data.Count; i++)
                        {
                            if (localRepository.ProcessChart.Changed)
                            {
                                MsgChartData msg = new MsgChartData();
                                msg.Data = localRepository.ProcessChart.Data;
                                PublishChart(msg);
                            }
                        }

                        await Task.Delay(interval);
                    }
                }
                catch (Exception exc)
                {
                    //TODO: Implementar framework de log
                }
            });
        }

        public void InitializeProcessChartUpdate(int interval)
        {
            ProcessChartUpdateLoop(interval);
        }

        public void ProcessChartUpdateLoop(int interval)
        {
            Task.Run(async delegate
            {
                try
                {
                    while (true)
                    {
                        LocalRepository localRepository = LocalRepository.GetInstance();

                        ProcessChartData data = new ProcessChartData();
                        for (int i = 0; i < localRepository.Tanks.Length; i++)
                        {
                            data.Level[i] = localRepository.Tanks[i].Level;
                        }
                        data.OperationMode = (int)localRepository.Pump.OperationMode;
                        data.PumpStatus = (int)localRepository.Pump.Status;
                        localRepository.ProcessChart.Add(data);

                        await Task.Delay(interval);
                    }
                }
                catch (Exception exc)
                {
                    //TODO: Implementar framework de log
                }
            });
        }

        public void InitializeProcessChartStoragerTimer(int interval)
        {
            _ProcessChartStoragerTimer = new System.Timers.Timer(interval);
            _ProcessChartStoragerTimer.Elapsed += SaveProcessChart;
            _ProcessChartStoragerTimer.Elapsed += ClearMemoryProcessChart;
            _ProcessChartStoragerTimer.AutoReset = true;
            _ProcessChartStoragerTimer.Enabled = true;
        }

        public void SaveProcessChart(object sender, ElapsedEventArgs e)
        {
            LocalRepository localRepository = LocalRepository.GetInstance();

            _RepositoryService.InsertProcessChart(localRepository.ProcessChart);
        }

        public void ClearMemoryProcessChart(object sender, ElapsedEventArgs e)
        {
            LocalRepository localRepository = LocalRepository.GetInstance();

            localRepository.ProcessChart.Data.Clear();
            localRepository.ProcessChart.Changed = true;
        }

        private void PublishChart(MsgChartData msg)
        {
            Program.FrmMain.UpdateChart(msg);
        }

        private void PublishDataWaterTank(MsgWaterTankData[] msgs)
        {
            Program.FrmMain.UpdateWaterTanks(msgs);
        }

        private void PublishDataPump(MsgPumpData msg)
        {
            Program.FrmMain.UpdatePump(msg);
        }

        public void SendPumpData(MsgPumpData msg)
        {
            LocalRepository localRepository = LocalRepository.GetInstance();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (msg.StatusPump == EnumPumpStatus.On)
                        localRepository.Pump.TurnOnPump();
                    else
                        localRepository.Pump.TurnOffPump();

                    if (msg.OperationMode == EnumOperationMode.Automatic)
                        localRepository.Pump.OperationMode = EnumOperationMode.Automatic;
                    else
                        localRepository.Pump.OperationMode = EnumOperationMode.Manual;

                    Program.UartService.SendMessage(new MsgTelegram200((int)msg.StatusPump, (int)msg.OperationMode));
                }
                catch (Exception e)
                {
                    //TODO: Implementar framework de log
                }
            });
        }

        public IEnumerable<ProcessChart> GetProcessCharts(DateTime startDate, DateTime endDate)
        {
            return _RepositoryService.GetProcessChartByPeriod(startDate, endDate);
        }
    }
}
