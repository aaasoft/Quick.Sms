using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Quick.Sms.Desktop.Controls;

namespace Quick.Sms.Desktop.ViewModels
{
    public class MainWindowViewModel : PropertyNotifyModel
    {
        private ISmsDevice device;

        public MessageBoxViewModel MessageBox { get; set; } = new MessageBoxViewModel()
        {
            ButtonOkText = "确定",
        };

        public string Title { get; set; }
        public SmsDeviceTypeInfo[] DeviceTypeInfos { get; set; }
        public Queue<string> LogQueue { get; set; } = new Queue<string>();
        public string Logs => string.Join(Environment.NewLine, LogQueue);

        public DelegateCommand HelpCommand { get; set; }
        public DelegateCommand OpenCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public DelegateCommand ScanCommand { get; set; }
        public DelegateCommand SendCommand { get; set; }
        public DelegateCommand RefreshAllStatusCommand { get; set; }
        public DelegateCommand SendATCommand { get; set; }

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

        //URL
        private string _Url = "serial://./COM1?BaudRate=115200";
        public string Url
        {
            get { return _Url; }
            set
            {
                _Url = value;
                RaisePropertyChanged();
                OpenCommand.RaiseCanExecuteChanged();
                ScanCommand.RaiseCanExecuteChanged();
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
        private string _SendContent = "{device},{time}";
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
        private SmsDeviceStatusViewModel[] _StatusInfos;
        public SmsDeviceStatusViewModel[] StatusInfos
        {
            get { return _StatusInfos; }
            set
            {
                _StatusInfos = value;
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
            Assembly assembly = GetType().Assembly;
            Title = $"{assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} v{assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
            DeviceTypeInfos = SmsDeviceManager.Instnce.GetDeviceTypeInfos();
            HelpCommand = new DelegateCommand() { ExecuteCommand = executeCommand_HelpCommand };
            OpenCommand = new DelegateCommand() { ExecuteCommand = executeCommand_OpenCommand, CanExecuteCommand = t => !string.IsNullOrEmpty(Url) };
            CloseCommand = new DelegateCommand() { ExecuteCommand = executeCommand_CloseCommand };
            ScanCommand = new DelegateCommand() { ExecuteCommand = executeCommand_ScanCommand, CanExecuteCommand = t => !string.IsNullOrEmpty(Url) };
            SendCommand = new DelegateCommand() { ExecuteCommand = executeCommand_SendCommand };
            RefreshAllStatusCommand = new DelegateCommand() { ExecuteCommand = executeCommand_RefreshAllStatusCommand };
            SendATCommand = new DelegateCommand() { ExecuteCommand = executeCommand_SendATCommand };
        }

        private void pushLog(string log)
        {
            var newLine = $"{DateTime.Now.ToString("HH:mm:ss.ffff")} {log}";
            LogQueue.Enqueue(newLine);
            RaisePropertyChanged(nameof(Logs));
        }

        private async Task OpenSerialPort()
        {
            device = SmsDeviceManager.Instnce.CreateDeviceInstance(DeviceType.Id,
                new SerialPortModemSetting()
                {
                    Url = Url
                });
            device.LineSended += (sender, line) => pushLog("TX " + line);
            device.LineRecved += (sender, line) => pushLog("RX " + line);

            await Task.Run(() => device.Open());
            StatusInfos = device.Status.Select(t => new SmsDeviceStatusViewModel(MessageBox)
            {
                Status = t,
                Value = string.Empty
            }).ToArray();
            IsOpen = true;
        }

        private void CloseSerialPort()
        {
            try { device?.Close(); } catch { }
            IsOpen = false;
            LogQueue.Clear();
        }

        private async void executeCommand_HelpCommand(object e)
        {
            MessageBox.Show("帮助", @"串口示例：
Windows:
serial://./COM1?BaudRate=115200
Linux:
serial://./dev/ttyS1?BaudRate=115200

TCP示例：
tcp://192.168.1.123:3001");
        }

        private async void executeCommand_OpenCommand(object e)
        {
            if (string.IsNullOrEmpty(Url))
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
                MessageBox.Loading("打开串口", $"正在打开串口[{Url}]...");
                await OpenSerialPort();
                MessageBox.Close();
            }
            catch (Exception ex)
            {
                CloseSerialPort();
                MessageBox.Show("错误", $"串口[{Url}]打开失败，原因：{ex.Message}");
            }
        }

        private void executeCommand_CloseCommand(object e)
        {
            CloseSerialPort();
        }

        private async void executeCommand_ScanCommand(object e)
        {
            try
            {
                if (string.IsNullOrEmpty(Url))
                {
                    MessageBox.Show("错误", $"请先选择串口!");
                    return;
                }
                MessageBox.Loading("智能识别", "正在识别中...");
                var deviceTypeInfo = await Task.Run(() => AbstractSerialPortModem.Scan(Url));
                DeviceType = deviceTypeInfo;
                MessageBox.Show("成功", $"已成功识别为[{deviceTypeInfo.Name}]!");
            }
            catch (Exception ex)
            {
                DeviceType = null;
                MessageBox.Show("失败", $"识别失败!{Environment.NewLine}{ex.Message}");
            }
        }

        private async void executeCommand_SendCommand(object e)
        {
            if (string.IsNullOrEmpty(SendTo))
            {
                MessageBox.Show("错误", $"请输入要发送到的号码!");
                return;
            }
            if (string.IsNullOrEmpty(SendContent))
            {
                MessageBox.Show("错误", $"请输入短信内容!");
                return;
            }
            var content = SendContent;
            content = content.Replace("{url}", Url);
            content = content.Replace("{device}", device.Name);
            content = content.Replace("{time}", DateTime.Now.ToString());
            content = content.Replace("{guid}", Guid.NewGuid().ToString("N"));

            try
            {
                MessageBox.Loading("发送短信中", $"正在向[{SendTo}]发送短信...");
                await Task.Run(() => device.Send(SendTo, content));
                MessageBox.Show("成功", $"发送短信成功。");
            }
            catch (Exception ex)
            {
                MessageBox.Show("失败", $"发送短信失败，原因：{ex.Message}");
            }
        }

        private async void executeCommand_RefreshAllStatusCommand(object e)
        {
            foreach (var statusInfo in StatusInfos)
                await statusInfo.ReadStatus(false);
            MessageBox.Close();
        }

        private void executeCommand_SendATCommand(object e)
        {
            if (string.IsNullOrEmpty(CommandText))
            {
                MessageBox.Show("错误", "请输入指令内容!");
                return;
            }
            device.ExecuteCommand(CommandText);
        }
    }
}
