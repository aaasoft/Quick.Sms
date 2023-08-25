using Quick.Sms.Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.Avalonia.ViewModels
{
    public class MainWindowViewModel : PropertyNotifyModel
    {
        private ISmsDevice device;

        public MessageBoxViewModel MessageBox { get; set; } = new MessageBoxViewModel()
        {
            ButtonOkText = "确定",
        };

        public string Title { get; set; }
        public string[] PortNames { get; set; }
        public SmsDeviceTypeInfo[] DeviceTypeInfos { get; set; }

        public DelegateCommand OpenCommand { get; set; }
        public DelegateCommand ScanCommand { get; set; }

        private bool _IsOpen = false;
        /// <summary>
        /// 串口是否已打开
        /// </summary>
        public bool IsOpen
        {
            get { return _IsOpen; }
            private set
            {
                _IsOpen = value;
                RaisePropertyChanged();
            }
        }

        //串口
        private string _PortName;
        public string PortName
        {
            get { return _PortName; }
            set
            {
                _PortName = value;
                RaisePropertyChanged();
                OpenCommand.RaiseCanExecuteChanged();
                ScanCommand.RaiseCanExecuteChanged();
            }
        }
        //波特率
        private int _BaudRate = 115200;
        public int BaudRate
        {
            get { return _BaudRate; }
            set
            {
                _BaudRate = value;
                RaisePropertyChanged();
            }
        }
        //设备类型
        private SmsDeviceTypeInfo _DeviceType;
        public SmsDeviceTypeInfo DeviceType
        {
            get { return _DeviceType; }
            set
            {
                _DeviceType = value;
                RaisePropertyChanged();
            }
        }
        private string _SendTo;
        /// <summary>
        /// 发送到
        /// </summary>
        public string SendTo
        {
            get { return _SendTo; }
            set
            {
                _SendTo = value;
                RaisePropertyChanged();
            }
        }
        private string _SendContent = "{device}({portName},{baudRate}),{time}";
        /// <summary>
        /// 发送内容
        /// </summary>
        public string SendContent
        {
            get { return _SendContent; }
            set
            {
                _SendContent = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// 状态字典
        /// </summary>
        private Dictionary<SmsDeviceStatus, string> _StatusDict;
        public Dictionary<SmsDeviceStatus, string> StatusDict
        {
            get { return _StatusDict; }
            set
            {
                _StatusDict = value;
                RaisePropertyChanged();
            }
        }

        private string _CommandText;
        /// <summary>
        /// 命令内容
        /// </summary>
        public string CommandText
        {
            get { return _CommandText; }
            set
            {
                _CommandText = value;
                RaisePropertyChanged();
            }
        }

        public MainWindowViewModel()
        {
            var assembly = GetType().Assembly;
            Title = $"{assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} v{assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
            PortNames = System.IO.Ports.SerialPort.GetPortNames();
            DeviceTypeInfos = SmsDeviceManager.Instnce.GetDeviceTypeInfos();            

            OpenCommand = new DelegateCommand() { ExecuteCommand = executeCommand_OpenCommand, CanExecuteCommand = t => !string.IsNullOrEmpty(PortName) };
            ScanCommand = new DelegateCommand() { ExecuteCommand = executeCommand_ScanCommand, CanExecuteCommand = t => !string.IsNullOrEmpty(PortName) };
        }


        private async Task OpenSerialPort()
        {
            device = SmsDeviceManager.Instnce.CreateDeviceInstance(DeviceType.Id,
                new SerialPortModemSetting()
                {
                    PortName = PortName,
                    BaudRate = BaudRate
                });
            //device.LineSended += (sender, line) => pushLog("TX " + line);
            //device.LineRecved += (sender, line) => pushLog("RX " + line);

            await Task.Run(() => device.Open());
            StatusDict = device.Status.ToDictionary(t => t, t => String.Empty);
            IsOpen = true;
        }

        private void CloseSerialPort()
        {
            try { device?.Close(); } catch { }
            IsOpen = false;
            //logViewControl?.Clear();
        }

        private async void executeCommand_OpenCommand(object e)
        {
            if (string.IsNullOrEmpty(PortName))
            {
                MessageBox.Show("错误", $"请先选择串口!");
                return;
            }
            if (DeviceType == null)
            {
                MessageBox.Show("错误", $"请先选择类型!");
                return;
            }

            try
            {
                MessageBox.Loading("打开串口", $"正在打开串口[{PortName}]...");
                await OpenSerialPort();
                MessageBox.Close();
            }
            catch (Exception ex)
            {
                CloseSerialPort();
                MessageBox.Show("错误", $"串口[{PortName}]打开失败，原因：{ex.Message}");
            }
        }

        private async void executeCommand_ScanCommand(object e)
        {
            try
            {
                if (string.IsNullOrEmpty(PortName))
                {
                    MessageBox.Show("错误", $"请先选择串口!");
                    return;
                }
                MessageBox.Loading("智能识别", "正在识别中...");
                var deviceTypeInfo = await Task.Run(() => AbstractSerialPortModem.Scan(PortName, BaudRate));
                DeviceType = deviceTypeInfo;
                MessageBox.Show("成功", $"已成功识别为[{deviceTypeInfo.Name}]!");
            }
            catch (Exception ex)
            {
                DeviceType = null;
                MessageBox.Show("失败", $"识别失败!{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
