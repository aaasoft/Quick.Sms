using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quick.Sms
{
    public abstract class AbstractSerialPortModem : ISmsDevice
    {
        private byte[] buffer = new byte[1 * 1024 * 1024];
        private Queue<string> readLinesQueue = new Queue<string>();

        public event EventHandler<string> LineSended;
        public event EventHandler<string> LineRecved;
        public event EventHandler<string> Warning;

        public void RaiseEventWaring(string message)
        {
            Warning?.Invoke(this, message);
        }

        public virtual bool SliceLongText => true;
        public virtual bool SupportResponseHead => false;
        public abstract string Name { get; }
        /// <summary>
        /// 写入数据换行符
        /// </summary>
        public virtual string WriteNewLine { get; } = "\r";
        /// <summary>
        /// 读取数据换行符
        /// </summary>
        public virtual string ReadNewLine { get; } = "\r\n";
        /// <summary>
        /// 是否支持UCS2字符集
        /// </summary>
        public virtual bool SupportCharterSet_UCS2 { get; } = true;
        
        protected SerialPort SerialPort { get; set; }

        public Type SettingType => typeof(SerialPortModemSetting);
        protected SerialPortModemSetting Setting { get; private set; }
        
        public virtual SmsDeviceFeature[] Features => new SmsDeviceFeature[]
        {
            new SmsDeviceFeature()
            {
                Name="接听",
                Action = Feature_Answer
            },
            new SmsDeviceFeature()
            {
                Name="挂断",
                Action = Feature_Hangup
            }
        };

        public SmsDeviceStatus Status_CGMI { get; private set; }
        public SmsDeviceStatus Status_CGMM { get; private set; }
        public SmsDeviceStatus Status_IPR_CURRENT { get; private set; }
        public SmsDeviceStatus Status_IPR_LIST { get; private set; }
        public SmsDeviceStatus Status_CGMR { get; private set; }
        public SmsDeviceStatus Status_CGSN { get; private set; }
        public SmsDeviceStatus Status_CIMI { get; private set; }
        public SmsDeviceStatus Status_COPS { get; private set; }
        public SmsDeviceStatus Status_GCAP { get; private set; }
        public SmsDeviceStatus Status_CREG_ZHCN { get; private set; }
        public SmsDeviceStatus Status_CCLK { get; private set; }
        public SmsDeviceStatus Status_CSQ_ZHCN { get; private set; }
        public SmsDeviceStatus Status_CSCA { get; private set; }

        public AbstractSerialPortModem()
        {
            Status_IPR_CURRENT = new SmsDeviceStatus() { Name = "当前波特率", Read = () => ExecuteCommand("AT+IPR?", "+IPR:") };
            Status_IPR_LIST = new SmsDeviceStatus() { Name = "支持波特率", Read = () => ExecuteCommand("AT+IPR=?", "+IPR:") };
            Status_CGMI = new SmsDeviceStatus() { Name = "生产商", Read = () => this.ExecuteCommand("AT+CGMI", "+CGMI:") };
            Status_CGMM = new SmsDeviceStatus() { Name = "型号", Read = () => ExecuteCommand("AT+CGMM", "+CGMM:") };
            Status_CGMR = new SmsDeviceStatus() { Name = "版本", Read = () => ExecuteCommand("AT+CGMR", "+CGMR:") };
            Status_CGSN = new SmsDeviceStatus() { Name = "IMEI", Read = () => ExecuteCommand("AT+CGSN", "+CGSN:") };
            Status_CIMI = new SmsDeviceStatus() { Name = "IMSI", Read = () => ExecuteCommand("AT+CIMI", "") };
            Status_COPS = new SmsDeviceStatus()
            {
                Name = "运营商",
                Read = () =>
                {
                    var line = ExecuteCommand("AT+COPS?", "+COPS:");
                    if (IsERROR(line))
                        return line;
                    var strs = line.Split(',');
                    if (strs.Length < 2)
                        return line;
                    StringBuilder sb = new StringBuilder();
                    var operaterName = strs[2];
                    operaterName = operaterName.Replace("\"", "");
                    sb.Append(operaterName);
                    if (strs.Length > 3)
                    {
                        string act = strs[3];
                        switch (act)
                        {
                            case "0":
                                act = "GSM";
                                break;
                            case "1":
                                act = "GSM Compact";
                                break;
                            case "2":
                                act = "UTRAN";
                                break;
                            case "7":
                                act = "EUTRAN";
                                break;
                            case "8":
                                act = "CDMA/HDR";
                                break;
                        }
                        sb.Append(",接入网:" + act);
                    }
                    return sb.ToString();
                }
            };
            Status_GCAP = new SmsDeviceStatus() { Name = "功能集", Read = () => ExecuteCommand("AT+GCAP", "+GCAP:") };            
            Status_CREG_ZHCN = new SmsDeviceStatus()
            {
                Name = "注册状态",
                Read = () =>
                {
                    var line = ExecuteCommand("AT+CREG?", "+CREG:");
                    if (IsERROR(line))
                        return line;
                    var strs = line.Split(',');
                    if (strs.Length < 2)
                        return line;
                    var stat = strs[1].Trim();
                    switch (stat)
                    {
                        case "0":
                            return "未注册,未搜索运营商";
                        case "1":
                            return "已注册,本地网络";
                        case "2":
                            return "未注册,正在搜索基站";
                        case "3":
                            return "拒绝注册";
                        case "4":
                            return "未知";
                        case "5":
                            return "已注册,漫游网络";
                        default:
                            return stat;
                    }
                }
            };
            Status_CCLK = new SmsDeviceStatus() { Name = "时钟", Read = () => ExecuteCommand("AT+CCLK?", "+CCLK:") };
            Status_CSQ_ZHCN = new SmsDeviceStatus()
            {
                Name = "信号强度",
                Read = () =>
                {
                    var line = ExecuteCommand("AT+CSQ", "+CSQ:");
                    var strs = line.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length < 2)
                        return line;
                    try
                    {
                        var val = int.Parse(strs[0]);
                        if (val == 99)
                            return "无信号";
                        return (val * 100F / 31).ToString("N0") + "%";
                    }
                    catch
                    {
                        return line;
                    }
                }
            };
            Status_CSCA = new SmsDeviceStatus()
            {
                Name = "短信中心",
                Read = () =>
                {
                    try
                    {
                        //先修改TE字符集为GSM
                        if (SupportCharterSet_UCS2)
                            write_Status_CSCS("GSM");
                        var response = this.ExecuteCommand("AT+CSCA?", "+CSCA:");
                        //最后修改TE字符集回UCS2
                        if (SupportCharterSet_UCS2)
                            write_Status_CSCS("UCS2");
                        if (IsERROR(response))
                            return response;
                        var strs = response.Split(',');
                        if (strs.Length < 2)
                            return response;
                        return strs[0].Replace("\"", "");
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                },
                Write = v =>
                  {
                      //先修改TE字符集为GSM
                      if (SupportCharterSet_UCS2)
                          write_Status_CSCS("GSM");
                      var response = this.ExecuteCommand($"AT+CSCA=\"{v}\"", null);
                      //最后修改TE字符集回UCS2
                      if (SupportCharterSet_UCS2)
                          write_Status_CSCS("UCS2");
                      if (IsERROR(response))
                          throw new ApplicationException(response);
                  }
            };
        }

        protected void write_Status_CSCS(string v)
        {
            var response = this.ExecuteCommand($"AT+CSCS=\"{v}\"", null);
            if (IsERROR(response))
                throw new ApplicationException(response);
        }

        public virtual SmsDeviceStatus[] Status => new SmsDeviceStatus[]
        {
            Status_CGMI,
            Status_CGMM,
            Status_IPR_CURRENT,
            Status_IPR_LIST,
            Status_CGMR,
            Status_CGSN,
            Status_CIMI,
            Status_COPS,
            Status_GCAP,
            Status_CREG_ZHCN,
            Status_CCLK,
            Status_CSQ_ZHCN
        };

        public virtual void Close()
        {
            SerialPort?.Close();
            SerialPort = null;
        }

        public virtual SerialPort CreateSerialPort()
        {
            return new SerialPort()
            {
                PortName = Setting.PortName,
                BaudRate = Setting.BaudRate,
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,
                RtsEnable = true,
                DtrEnable = true,
                ReadTimeout = Setting.ReadResponseTimeout,
                WriteTimeout = Setting.WriteCommandTimeout
            };
        }

        protected void ClearBuffer()
        {
            lock (this)
                readLinesQueue.Clear();
        }

        protected bool IsOK(string rep)
        {
            return rep == "OK";
        }

        protected bool IsERROR(string rep)
        {
            return rep == "ERROR";
        }

        public virtual void InitModem()
        {
            //清除缓冲区
            ClearBuffer();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                WriteLine("AT");
                var line = ReadLine();
                if (IsOK(line))
                    break;
                //如果等待时间大于了超时时间
                if (stopwatch.ElapsedMilliseconds > Setting.ReadResponseTimeout)
                {
                    stopwatch.Stop();
                    throw new TimeoutException();
                }
                //如果没有关闭回显，则关闭回显
                WriteCommand("ATE0");
            }
            Thread.Sleep(100);
            ClearBuffer();
        }

        public virtual void Init(object settingObj)
        {
            if (SerialPort != null)
                Close();
            Setting = (SerialPortModemSetting)settingObj;
            SerialPort = CreateSerialPort();
        }
        
        public virtual void Open()
        {
            SerialPort.Open();
            beginReadFromSerialPort();
            while (true)
            {
                Thread.Sleep(100);
                if (SerialPort.BytesToRead > 0)
                    SerialPort.Read(buffer, 0, SerialPort.BytesToRead);
                else
                    break;
            }
            ClearBuffer();
            //确保短信猫工作
            InitModem();
        }

        private void addReadLines(string[] lines)
        {
            lock (readLinesQueue)
            {
                foreach (var line in lines)
                {
                    readLinesQueue.Enqueue(line);
                    LineRecved?.Invoke(this, line);
                }
            }
        }

        private void beginReadFromSerialPort()
        {
            Task.Delay(100).ContinueWith(t =>
            {
                var serialPort = SerialPort;
                if (serialPort == null || !serialPort.IsOpen)
                    return;
                beginReadFromSerialPort();
                var readCount = serialPort.BytesToRead;
                if (readCount <= 0)
                    return;
                string[] lines = null;
                lock (this)
                {
                    try
                    {
                        int ret = 0;
                        string text = null;

                        while (true)
                        {
                            readCount = serialPort.BytesToRead;
                            if (readCount <= 0)
                                break;
                            ret += serialPort.Read(buffer, ret, buffer.Length - ret);
                            text = Encoding.ASCII.GetString(buffer, 0, ret);
                            if (text.EndsWith(ReadNewLine))
                                break;
                            Thread.Sleep(100);
                        }
                        if (text == null)
                            return;
                        lines = text.Split(new string[] { ReadNewLine }, StringSplitOptions.RemoveEmptyEntries);
                    }
                    catch { }
                }
                if (lines == null)
                    return;
                addReadLines(lines);
            });
        }
        public string ReadLine()
        {
            return ReadLine(Setting.ReadResponseTimeout);
        }

        public string ReadLine(int timeout)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (true)
            {
                //尝试读取一行数据并返回
                lock (readLinesQueue)
                    if (readLinesQueue.Count > 0)
                        return readLinesQueue.Dequeue();
                //如果等待时间大于了超时时间
                if (stopwatch.ElapsedMilliseconds > timeout)
                {
                    stopwatch.Stop();
                    throw new TimeoutException("读取数据超时");
                }
                //等待0.1秒再试
                Thread.Sleep(100);
            }
        }

        protected bool WriteCommand(byte[] commandBytes)
        {
            return WriteCommand(commandBytes, null, out _);
        }

        protected bool WriteCommand(byte[] commandBytes, string responseHead, out string response)
        {
            return WriteCommand(commandBytes, responseHead, Setting.ReadResponseTimeout, out response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="commandBytes"></param>
        /// <param name="responseHead">responseHead为null代表指令没有返回内容</param>
        /// <param name="response"></param>
        /// <returns></returns>
        protected bool WriteCommand(byte[] commandBytes, string responseHead, int timeout, out string response)
        {
            string line;
            response = null;

            Write(commandBytes);

            if (responseHead != null)
            {
                //读取指令响应
                while (true)
                {
                    line = ReadLine(timeout);
                    response = line;

                    //如果指令报错
                    if (IsERROR(line))
                        return false;

                    if (line.StartsWith(responseHead))
                    {
                        response = line.Substring(responseHead.Length).TrimStart();
                        break;
                    }

                    if (!SupportResponseHead)
                        break;
                }
            }
            //读取指令是否成功
            while (true)
            {
                line = ReadLine(timeout);
                if (response == null)
                    response = line;
                if (IsOK(line))
                    return true;
                if (IsERROR(line))
                    return false;
            }
        }

        protected bool WriteCommand(string command)
        {
            return WriteCommand(Encoding.Default.GetBytes(command + WriteNewLine));
        }

        protected bool WriteCommand(string command, string responseHead, out string response)
        {
            return WriteCommand(command, responseHead, Setting.ReadResponseTimeout, out response);
        }

        protected bool WriteCommand(string command, string responseHead, int timeout, out string response)
        {
            return WriteCommand(Encoding.Default.GetBytes(command + WriteNewLine), responseHead, timeout, out response);
        }

        public void WriteLine(string text)
        {
            Write(Encoding.Default.GetBytes(text + WriteNewLine));
        }

        public void Write(byte[] bytesToWrite)
        {
            var text = Encoding.Default.GetString(bytesToWrite);
            if (text.EndsWith(WriteNewLine))
                text = text.Substring(0, text.Length - WriteNewLine.Length);
            LineSended?.Invoke(this, text);
            SerialPort.Write(bytesToWrite, 0, bytesToWrite.Length);
        }

        /// <summary>
        /// 发送内容编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual byte[] EncodeContent(string content, Encoding encoding)
        {
            return encoding.GetBytes(content);
        }

        /// <summary>
        /// 发送内容编码
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual byte[] EncodeContent(string content)
        {
            return EncodeContent(content, Encoding.BigEndianUnicode);
        }

        public virtual void Send(string sendTo, string content)
        {
            //号码分隔
            var desArray = sendTo.Split(new string[] { ",", ";", " " }, StringSplitOptions.RemoveEmptyEntries);

            var smsMaxCount = 70;
            //短信按70字分隔
            string[] contentParts = null;
            if (!SliceLongText || content.Length <= smsMaxCount)
            {
                contentParts = new string[] { content };
            }
            else
            {
                List<string> list = new List<string>();
                for (var i = 0; i < content.Length; i += smsMaxCount)
                    list.Add(content.Substring(i, Math.Min(smsMaxCount, content.Length - i)));
                contentParts = list.ToArray();
            }

            foreach (var des in desArray)
                foreach (var contentPart in contentParts)
                {
                    InternalSend(des, contentPart);
                }
        }

        protected abstract void InternalSend(string sendTo, string content);

        //接听
        private void Feature_Answer()
        {
            WriteCommand("ATA");
        }

        //挂断
        private void Feature_Hangup()
        {
            WriteCommand("ATH");
        }

        public void ExecuteCommand(byte[] bytesToWrite)
        {
            Write(bytesToWrite);
        }

        public void ExecuteCommand(string command)
        {
            WriteLine(command);
        }

        public virtual string ExecuteCommand(string command, string responseHead)
        {
            //清空缓冲区
            ClearBuffer();
            string rep;
            WriteCommand(command, responseHead, out rep);
            return rep;
        }

        /// <summary>
        /// 设备特征符
        /// </summary>
        protected virtual string[] DeviceMarks => null;

        public static SmsDeviceTypeInfo Scan(string portName,int baudRate)
        {
            var testModem = new PrivateModem();
            try
            {
                testModem.Init(new SerialPortModemSetting() { PortName = portName, BaudRate = baudRate });
                testModem.Open();
                return Scan(testModem);
            }
            catch
            {
                throw;
            }
            finally
            {
                testModem.Close();
            }
        }

        public static SmsDeviceTypeInfo Scan(AbstractSerialPortModem testModem)
        {
            var text = testModem.GetDeviceInfo();
            foreach (var item in SmsDeviceManager.Instnce.GetMasterDeviceTypes())
            {
                var device = item as AbstractSerialPortModem;
                if (device == null)
                    continue;
                if (device.IsMatch(text))
                    return new SmsDeviceTypeInfo()
                    {
                        Id = device.GetType().FullName,
                        Name = device.Name
                    };
            }
            throw new ApplicationException(text);
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDeviceInfo()
        {
            ClearBuffer();
            var status = new[] { Status_CGMI, Status_CGMM, Status_CGMR };
            var text = string.Join(Environment.NewLine, status.Select(t => $"{t.Name}: {t.Read()}"));
            return text;
        }

        /// <summary>
        /// 检查生产商、型号、版本与当前设备是否匹配
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool IsMatch(string text)
        {
            var deviceMarks = DeviceMarks;
            if (deviceMarks == null)
                return false;

            var ret = deviceMarks.All(t1 =>
            {
                var strs = t1.Split('|');
                return strs.All(t2 => text.Contains(t2));
            });
            return ret;
        }

        public virtual void CheckIsMatch()
        {
            var text = GetDeviceInfo();
            if (!IsMatch(text))
                throw new ApplicationException(text);
        }

        private class PrivateModem : AbstractSerialPortModem
        {
            public override string Name => throw new NotImplementedException();

            protected override void InternalSend(string sendTo, string content)
            {
                throw new NotImplementedException();
            }
        }
    }
}
