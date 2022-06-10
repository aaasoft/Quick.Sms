using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms
{
    /// <summary>
    /// 短信猫设备状态
    /// </summary>
    public class SmsDeviceStatus
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 读取
        /// </summary>
        public Func<string> Read { get; set; }
        /// <summary>
        /// 写入
        /// </summary>
        public Action<string> Write { get; set; }
    }
}
