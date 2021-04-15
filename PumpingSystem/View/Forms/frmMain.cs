using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Data;
using System.Drawing;
using PumpingSystem.Messages.View;
using PumpingSystem.Common;

namespace PumpingSystem.View
{
    public partial class frmMain : Form, IUpdatableForm<Form>
    {
        private MsgWaterTankData[] _MsgsDataWaterTank = { new MsgWaterTankData(), new MsgWaterTankData() };
        private MsgPumpData _MsgDataPump = new MsgPumpData();
        private int _IndexLastRenderedPoint = 0;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            chtLevel.ChartAreas[0].AxisY.Maximum = 100;
            chtLevel.ChartAreas[0].AxisY.Interval = 10;
            chtLevel.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";            
            chtLevel.Series["Level1"].Points.AddXY(DateTime.Now, 0);
            chtLevel.Series["Level2"].Points.AddXY(DateTime.Now, 0);
        }

        public void UpdateWaterTanks(MsgWaterTankData[] msgs)
        {
            _MsgsDataWaterTank = msgs;
            UpdateLevel(EnumWaterTank.Tank1, msgs[(int)EnumWaterTank.Tank1].Level);
            UpdateLevel(EnumWaterTank.Tank2, msgs[(int)EnumWaterTank.Tank2].Level);
        }

        private void UpdateLevel(EnumWaterTank tank, int level)
        {
            PictureBox picWaterTank = tank == EnumWaterTank.Tank1 ? picWaterTank1 : picWaterTank2;
            TextBox txtLevel = tank == EnumWaterTank.Tank1 ? txtLevel1 : txtLevel2;

            MethodInvoker mth = (MethodInvoker)delegate ()
            {
                try
                {
                    txtLevel.Text = string.Format("{0:F0}%", level);

                    if (level < 10) picWaterTank.Image = Properties.Resources.water_tank_level_0;
                    else if (level < 20) picWaterTank.Image = Properties.Resources.water_tank_level_10;
                    else if (level < 30) picWaterTank.Image = Properties.Resources.water_tank_level_20;
                    else if (level < 40) picWaterTank.Image = Properties.Resources.water_tank_level_30;
                    else if (level < 50) picWaterTank.Image = Properties.Resources.water_tank_level_40;
                    else if (level < 60) picWaterTank.Image = Properties.Resources.water_tank_level_50;
                    else if (level < 70) picWaterTank.Image = Properties.Resources.water_tank_level_60;
                    else if (level < 80) picWaterTank.Image = Properties.Resources.water_tank_level_70;
                    else if (level < 90) picWaterTank.Image = Properties.Resources.water_tank_level_80;
                    else if (level < 100) picWaterTank.Image = Properties.Resources.water_tank_level_90;
                    else picWaterTank.Image = Properties.Resources.water_tank_level_100;
                }
                catch (Exception e)
                {
                    txtMsg.Text = e.Message;
                }
            };
            if (!this.IsHandleCreated)
                mth.Invoke();
            else
                this.BeginInvoke(mth);
        }

        public void UpdatePump(MsgPumpData msg)
        {
            _MsgDataPump = msg;

            MethodInvoker mth = (MethodInvoker)delegate ()
            {
                try
                {
                    btnPumpLed.BackgroundImage = msg.StatusPump == EnumPumpStatus.On ? Properties.Resources.led_on : Properties.Resources.led_off;
                    btnOnOff.BackgroundImage = msg.StatusPump == EnumPumpStatus.On ? Properties.Resources.button_on : Properties.Resources.button_off;
                    btnOperationMode.IsOn = msg.OperationMode == EnumOperationMode.Automatic ? true : false;
                    UpdatePipes(msg.StatusPump);
                }
                catch (Exception e)
                {
                    txtMsg.Text = e.Message;
                }
            };
            if (!this.IsHandleCreated)
                mth.Invoke();
            else
                this.BeginInvoke(mth);
        }

        private void UpdatePipes(EnumPumpStatus pumpStatus)
        {
            Task.Factory.StartNew(() =>
            {
                MethodInvoker mth = (MethodInvoker)delegate ()
                {
                    try
                    {
                        lock (this)
                        {
                            List<Control> controls = new List<Control>();
                            foreach (Control control in this.splMain.Panel2.Controls)
                            {
                                if (control.Name.StartsWith("pipe"))
                                {
                                    controls.Add(control);
                                }
                            }

                            if (pumpStatus == EnumPumpStatus.On)
                            {
                                foreach (Control control in controls.OrderBy(p => p.Tag))
                                {
                                    ((Panel)control).BackColor = Color.FromArgb(0, 168, 243);
                                    this.Refresh();
                                    Thread.Sleep(50);
                                }
                            }
                            else
                            {
                                foreach (Control control in controls.OrderByDescending(p => p.Tag))
                                {
                                    ((Panel)control).BackColor = Color.White;
                                    this.Refresh();
                                    Task.Delay(50);
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        txtMsg.Text = e.Message;
                    }
                };
                if (!this.IsHandleCreated)
                    mth.Invoke();
                else
                    this.BeginInvoke(mth);
            });
        }

        public void UpdateChart(MsgChartData msg)
        {
            MethodInvoker mth = (MethodInvoker)delegate ()
            {
                try
                {
                    while (_IndexLastRenderedPoint < msg.Data.Count)
                    {
                        ProcessChartData data = msg.Data[_IndexLastRenderedPoint];
                        if (_IndexLastRenderedPoint >= chtLevel.Series["Level1"].Points.Count)
                        {
                            chtLevel.Series["Level1"].Points.AddXY(data.SampleDate, data.Level[0]);
                            chtLevel.Series["Level2"].Points.AddXY(data.SampleDate, data.Level[1]);
                        }
                        ++_IndexLastRenderedPoint;
                    }
                }
                catch (Exception e)
                {
                    txtMsg.Text = e.Message;
                }
            };
            if (!this.IsHandleCreated)
                mth.Invoke();
            else
                this.BeginInvoke(mth);
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (_MsgDataPump.StatusPump == EnumPumpStatus.On)
                {
                    _MsgDataPump.StatusPump = EnumPumpStatus.Off;
                }
                else
                {
                    _MsgDataPump.StatusPump = EnumPumpStatus.On;
                }

                txtMsg.Text = _MsgDataPump.StatusPump == EnumPumpStatus.On ? "Bomba acionada." : "Bomba desligada.";

                UpdatePump(_MsgDataPump);
                Program.ViewService.SendPumpData(_MsgDataPump);
            }
            catch (Exception exc)
            {
                txtMsg.Text = exc.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void btnOperationMode_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (_MsgDataPump.OperationMode == EnumOperationMode.Automatic)
                {
                    _MsgDataPump.OperationMode = EnumOperationMode.Manual;
                }
                else
                {
                    _MsgDataPump.OperationMode = EnumOperationMode.Automatic;
                }

                txtMsg.Text = _MsgDataPump.OperationMode == EnumOperationMode.Automatic ? "Modo de operação automático acionado." : "Modo de operação manual acionado.";

                btnOperationMode.IsOn = _MsgDataPump.OperationMode == EnumOperationMode.Automatic ? true : false;
                Program.ViewService.SendPumpData(_MsgDataPump);
            }
            catch (Exception exc)
            {
                txtMsg.Text = exc.Message;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
