using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using PumpingSystem.Common;
using PumpingSystem.Messages.View;

namespace PumpingSystem.View
{
    public partial class frmMain : Form, IUpdateForm<Form>
    {
        private MsgDataWaterTank[] _MsgsDataWaterTank = { new MsgDataWaterTank(), new MsgDataWaterTank() };
        private MsgDataPump _MsgDataPump = new MsgDataPump();
        private MsgOperationMode _MsgOperationMode = new MsgOperationMode();

        public frmMain()
        {
            InitializeComponent();
        }

        public void UpdateWaterTanks(MsgDataWaterTank[] msgs)
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

        public void UpdatePump(MsgDataPump msg)
        {
            _MsgDataPump = msg;

            MethodInvoker mth = (MethodInvoker)delegate ()
            {
                try
                {
                    btnPumpLed.BackgroundImage = msg.StatusPump == EnumPumpStatus.On ? Properties.Resources.led_on : Properties.Resources.led_off;
                    btnOnOff.BackgroundImage = msg.StatusPump == EnumPumpStatus.On ? Properties.Resources.button_on : Properties.Resources.button_off;
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

        public void UpdateOperationMode(MsgOperationMode msg)
        {
            _MsgOperationMode = msg;

            MethodInvoker mth = (MethodInvoker)delegate ()
            {
                try
                {
                    btnOperationMode.IsOn = msg.OperationMode == EnumOperationMode.Automatic ? true : false;
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
            Application.DoEvents();

            Thread thread = new Thread(new ThreadStart(() =>
            {
                MethodInvoker mth = (MethodInvoker)delegate ()
                {
                    try
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
                                Thread.Sleep(50);
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
            }));
            thread.Start();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            join1.BringToFront();
            join7.BringToFront();
            join8.BringToFront();
            join10.BringToFront();
            pipe1.BringToFront();
            pipe5.BringToFront();
            pipe6.BringToFront();
            pipe7.BringToFront();
            txtLevel1.BringToFront();
            txtLevel2.BringToFront();
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
                Program.InterfaceView.SendPumpStatus(_MsgDataPump);
            }
            catch (Exception exc)
            {
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

                if (_MsgOperationMode.OperationMode == EnumOperationMode.Automatic)
                {
                    _MsgOperationMode.OperationMode = EnumOperationMode.Manual;
                }
                else
                {
                    _MsgOperationMode.OperationMode = EnumOperationMode.Automatic;
                }

                txtMsg.Text = _MsgOperationMode.OperationMode == EnumOperationMode.Automatic ? "Modo de operação automático acionado." : "Modo de operação manual acionado.";

                UpdateOperationMode(_MsgOperationMode);
                Program.InterfaceView.SendOperationMode(_MsgOperationMode);
            }
            catch (Exception exc)
            { 
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
    }
}
