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
        public string Title { get; set; }
        public string[] PortNames { get; set; }
        public SmsDeviceTypeInfo[] DeviceTypeInfos { get; set; }

        //串口
        private string _PortName;
        public string PortName
        {
            get { return _PortName; }
            set
            {
                _PortName = value;
                RaisePropertyChanged();
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

        public MainWindowViewModel()
        {
            var assembly = GetType().Assembly;
            Title = $"{assembly.GetCustomAttribute<AssemblyProductAttribute>().Product} v{assembly.GetCustomAttribute<AssemblyFileVersionAttribute>().Version}";
            PortNames = System.IO.Ports.SerialPort.GetPortNames();
            PortName = PortNames.FirstOrDefault();
            DeviceTypeInfos = SmsDeviceManager.Instnce.GetDeviceTypeInfos();
            DeviceType = DeviceTypeInfos.FirstOrDefault();
        }
    }
}
