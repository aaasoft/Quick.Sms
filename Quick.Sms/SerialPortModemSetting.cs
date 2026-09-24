using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms
{
    public class SerialPortModemSetting
    {
        /// <summary>
        /// 端口名称
        /// </summary>
        public string PortName { get; set; }
        /// <summary>
        /// 波特率
        /// </summary>
        public int BaudRate { get; set; } = 115200;
        /// <summary>
        /// 发送指令超时时间
        /// </summary>
        public int WriteCommandTimeout { get; set; } = 10 * 1000;
        /// <summary>
        /// 读取响应超时时间
        /// </summary>
        public virtual int ReadResponseTimeout { get; set; } = 10 * 1000;
        /// <summary>
        /// 短信发送超时时间
        /// </summary>
        public virtual int SmsSendTimeout { get; set; } = 60 * 1000;
    }
}
