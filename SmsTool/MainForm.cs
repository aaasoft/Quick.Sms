using Quick.Sms;
using SmsTool.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmsTool
{
    public partial class MainForm : Form
    {
        private bool isOpen = false;
        private ISmsDevice device = null;

        public MainForm()
        {
            InitializeComponent();
            this.Text = $"{Application.ProductName} v{Application.ProductVersion}";
        }

        private void SetLoading(bool loading, string content = "就绪")
        {
            gbSetting.Enabled = !loading;
            tabTestAndLog.Enabled = !loading;
            lblStatus.Text = content;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            cbDeviceType.ValueMember = nameof(SmsDeviceTypeInfo.Id);
            cbDeviceType.DisplayMember = nameof(SmsDeviceTypeInfo.Name);
            cbDeviceType.DataSource = new SmsDeviceTypeInfo[]
            {
                new SmsDeviceTypeInfo()
                {
                    Id=null,
                    Name="请选择..."
                }
            }.Concat(SmsDeviceManager.Instnce.GetDeviceTypeInfos()).ToArray();
            btnRefreshSerialPorts_Click(this, EventArgs.Empty);
            this.Enabled = true;
        }

        private async void BtnOpen_Click(object sender, EventArgs e)
        {
            if (isOpen)
            {
                CloseSerialPort();
            }
            else
            {
                var deviceInfo = (SmsDeviceTypeInfo)cbDeviceType.SelectedItem;
                if (deviceInfo.Id == null)
                {
                    MessageBox.Show($"请选择设备类型！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                var portName = cbSerialPort.SelectedItem.ToString();
                var baudRate = Convert.ToInt32(nudBaudRate.Value);
                try
                {
                    SetLoading(true, "正在打开...");
                    await OpenSerialPort(deviceInfo.Id, portName, baudRate);
                }
                catch (Exception ex)
                {
                    CloseSerialPort();
                    MessageBox.Show($"串口[{cbSerialPort.SelectedItem}]打开失败，原因：{ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                finally
                {
                    SetLoading(false);
                }
            }
        }

        private void pushLog(string log)
        {
            if (txtLogs.TextLength > 0)
                txtLogs.AppendText(Environment.NewLine);
            //去掉结尾的\r字符，防止主线程阻塞
            while (log.EndsWith("\r"))
                log = log.Substring(0, log.Length - 1);
            txtLogs.AppendText($"{DateTime.Now.ToString("HH:mm:ss.ffff")} {log}");
        }

        private async Task OpenSerialPort(string deviceTypeId, string portName, int baudRate)
        {
            cbDeviceType.Enabled = false;
            cbSerialPort.Enabled = false;
            nudBaudRate.Enabled = false;

            device = SmsDeviceManager.Instnce.CreateDeviceInstance(deviceTypeId,
                new SerialPortModemSetting()
                {
                    PortName = portName,
                    BaudRate = baudRate
                });
            device.LineSended += (sender, line) => BeginInvoke(new Action(() => pushLog("TX " + line)));
            device.LineRecved += (sender, line) => BeginInvoke(new Action(() => pushLog("RX " + line)));

            await Task.Run(() => device.Open());

            //加载设备功能
            flpFeatures.Controls.Clear();
            foreach (var feature in device.Features)
            {
                var btn = new Button() { Text = feature.Name };
                btn.Click += (sender, e) =>
                {
                    try
                    {
                        feature.Action();
                        MessageBox.Show($"执行功能[{feature.Name}]成功！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"执行功能[{feature.Name}]失败！原因：{ex.ToString()}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                };
                flpFeatures.Controls.Add(btn);
            }
            //加载设备状态
            flpDeviceStatus.Controls.Clear();
            flpDeviceStatus.Controls.Add(btnRefreshDeviceStatus);
            foreach (var status in device.Status)
            {
                var canWrite = status.Write != null;
                Panel pnl = new Panel() { Width = btnRefreshDeviceStatus.Width, Height = 24, Margin = new Padding(1) };
                Label lbl = new Label() { Text = status.Name, Dock = DockStyle.Left, BackColor = SystemColors.ActiveCaption, TextAlign = ContentAlignment.MiddleCenter, Margin = new Padding(0) };
                TextBox textBox = null;
                List<Button> btnList = new List<Button>();
                //读按钮
                {
                    var btn = new Button() { Text = "读", Dock = DockStyle.Right, Width = 36, Height = 36, Margin = new Padding(0) };
                    btnList.Add(btn);
                    btn.Click += async (sender, e) =>
                    {
                        try
                        {
                            btn.Enabled = false;
                            textBox.Text = "获取中...";
                            string statusText = null;
                            await Task.Run(() => statusText = status.Read());
                            textBox.Text = statusText;
                        }
                        catch (Exception ex)
                        {
                            textBox.Text = "读取失败，原因：" + ex.Message;
                        }
                        finally
                        {
                            btn.Enabled = true;
                        }
                    };
                }
                //写按钮
                if (canWrite)
                {
                    var btn = new Button() { Text = "写", Dock = DockStyle.Right, Width = 36, Height = 36, Margin = new Padding(0) };
                    btnList.Add(btn);
                    btn.Click += (sender, e) =>
                    {
                        var v = textBox.Text.Trim();
                        try
                        {
                            textBox.Text = "写入中...";
                            Application.DoEvents();
                            status.Write(v);
                            Application.DoEvents();
                            MessageBox.Show($"写入[{status.Name}]成功！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"写入[{status.Name}]失败！原因：{ex}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                        textBox.Text = v;
                    };
                }
                textBox = new TextBox()
                {
                    ReadOnly = !canWrite,
                    BorderStyle = canWrite ? BorderStyle.Fixed3D : BorderStyle.None,
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Top = canWrite ? 2 : 8,
                    Left = lbl.Width + 2,
                    Width = btnRefreshDeviceStatus.Width - lbl.Width - btnList.Sum(t => t.Width) - 2
                };
                pnl.Controls.Add(textBox);
                pnl.Controls.Add(lbl);
                foreach (var btn in btnList)
                    pnl.Controls.Add(btn);
                flpDeviceStatus.Controls.Add(pnl);
            }
            isOpen = true;
            btnScan.Visible = false;
            btnOpen.Text = "关闭";
            tabTestAndLog.Visible = true;
        }

        private void CloseSerialPort()
        {
            tabTestAndLog.Visible = false;
            try { device?.Close(); } catch { }
            isOpen = false;

            txtLogs.Clear();
            btnScan.Visible = true;
            flpFeatures.Controls.Clear();
            flpDeviceStatus.Controls.Clear();

            btnOpen.Text = "打开";
            cbDeviceType.Enabled = true;
            cbSerialPort.Enabled = true;
            nudBaudRate.Enabled = true;
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            var sendTo = txtSendTo.Text.Trim();
            if (string.IsNullOrEmpty(sendTo))
            {
                MessageBox.Show("未输入发送到号码！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var sendToArray = sendTo.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            var sendContent = txtSendContent.Text.Trim();
            if (string.IsNullOrEmpty(sendContent))
            {
                MessageBox.Show("未输入发送内容！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int sendCount = Convert.ToInt32(nudSendCount.Value);
            SendStatusForm statusForm = new SendStatusForm();
            statusForm.Size = Size;

            this.Enabled = false;
            Task.Run(async () =>
            {
                while (!statusForm.IsHandleCreated)
                    await Task.Delay(1000);
                int successCount = 0;
                int failedCount = 0;
                try
                {
                    statusForm.PushLog($"发送目标：{sendTo}");
                    statusForm.PushLog($"发送内容：{sendContent}");
                    statusForm.PushLog($"发送份数：{sendCount}");
                    statusForm.PushLog("------------------------------------");
                    statusForm.SetMaximum(sendToArray.Length * sendCount);
                    Stopwatch stopwatch = new Stopwatch();
                    for (var i = 0; i < sendCount; i++)
                    {
                        for (var j = 0; j < sendToArray.Length; j++)
                        {
                            var currentSendTo = sendToArray[j];
                            statusForm.PushLog($"开始向[{currentSendTo}]发送短信...");
                            statusForm.PushProcess(i * sendToArray.Length + j);
                            var content = sendContent;
                            content = content.Replace("{i}", (i + 1).ToString());
                            content = content.Replace("{guid}", Guid.NewGuid().ToString());

                            Exception sendException = null;
                            stopwatch.Restart();

                            for (var n = 0; n < 3; n++)
                            {
                                sendException = null;
                                try
                                {
                                    await Task.Run(() =>
                                    {
                                        device.Send(currentSendTo, content);
                                    });
                                }
                                catch (Exception ex)
                                {
                                    sendException = ex;
                                }
                                Thread.Sleep(1000);
                                if (sendException == null)
                                    break;
                                statusForm.PushLog($"第{n + 1}次发送到[{currentSendTo}]失败！，错误：{sendException.Message}");
                            }
                            stopwatch.Stop();
                            if (sendException == null)
                            {
                                successCount++;
                                statusForm.PushLog($"发送到[{currentSendTo}]成功！用时：{stopwatch.Elapsed}");
                            }
                            else
                            {
                                failedCount++;
                                statusForm.PushLog($"发送到[{currentSendTo}]失败！用时：{stopwatch.Elapsed}，错误：{sendException.Message}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    statusForm.PushLog("发送失败，原因：" + ex.Message);
                }
                finally
                {
                    statusForm.PushProcess(sendToArray.Length * sendCount);
                    statusForm.PushLog("------------------------------------");
                    statusForm.PushLog($"全部短信发送完成。其中成功[{successCount}]条，失败{failedCount}条。");
                    statusForm.Invoke(new Action(() =>
                    {
                        statusForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                    }));
                }
            });
            statusForm.ShowDialog();
            this.Enabled = true;
        }

        private async void BtnRefreshDeviceStatus_Click(object sender, EventArgs e)
        {
            foreach (var control in flpDeviceStatus.Controls)
            {
                if (!(control is Panel))
                    continue;
                if (control == btnRefreshDeviceStatus)
                    continue;
                var panel = (Panel)control;
                var btn = (Button)panel.Controls[2];
                btn.PerformClick();
                await Task.Run(() =>
                {
                    while (!btn.Enabled)
                        Thread.Sleep(100);
                });
            }
        }

        private void BtnClearLogs_Click(object sender, EventArgs e)
        {
            txtLogs.Clear();
        }

        private void BtnSendAT_Click(object sender, EventArgs e)
        {
            var commandText = txtATCommand.Text.Trim();
            if (string.IsNullOrEmpty(commandText))
            {
                MessageBox.Show("未输入要发送的指令！", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cbSendEncoding.Text != "16进制")
            {
                device.ExecuteCommand(commandText);
                return;
            }
            if (!(device is AbstractSerialPortModem modem))
                return;
            var bytes = commandText.ToHex();
            modem.Write(bytes);
        }

        private void FlpDeviceStatus_SizeChanged(object sender, EventArgs e)
        {
            btnRefreshDeviceStatus.Width = flpDeviceStatus.Width - 24;
            foreach (Control control in flpDeviceStatus.Controls)
            {
                if (!(control is Panel))
                    continue;
                if (control == btnRefreshDeviceStatus)
                    continue;

                var panel = (Panel)control;
                panel.Width = btnRefreshDeviceStatus.Width;
            }
        }

        private async void BtnScan_Click(object sender, EventArgs e)
        {
            try
            {
                SetLoading(true, "智能识别中...");
                var portName = cbSerialPort.SelectedItem.ToString();
                var baudRate = Convert.ToInt32(nudBaudRate.Value);

                var deviceTypeInfo = await Task.Run(() => AbstractSerialPortModem.Scan(portName, baudRate));
                cbDeviceType.SelectedValue = deviceTypeInfo.Id;
                MessageBox.Show($"已成功识别为[{deviceTypeInfo.Name}]!", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                cbDeviceType.SelectedItem = null;
                MessageBox.Show($"识别失败!{Environment.NewLine}{ex.Message}", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void TxtATCommand_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                btnSendAT.PerformClick();
        }

        private void btnRefreshSerialPorts_Click(object sender, EventArgs e)
        {
            cbSerialPort.DataSource = SerialPort.GetPortNames();
            cbSendEncoding.SelectedIndex = 0;
        }
    }
}
