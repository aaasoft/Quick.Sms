using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms
{
    /// <summary>
    /// 短信设备功能
    /// </summary>
    public class SmsDeviceFeature
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 动作
        /// </summary>
        public Action Action { get; set; }
    }
}
