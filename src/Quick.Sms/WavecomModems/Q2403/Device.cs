using Quick.Sms.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.WavecomModems.Q2403
{
    public class Device : AbstractSerialPortModem
    {
        //内容尾巴
        private byte[] contentTail = new byte[] { 0x00, 0x1a };

        public override string Name => "Wavecom_Q2403系列芯片";
        /*
            Wavecom GPRS芯片
            -----------------------------
            生产商:  WAVECOM MODEM
            型号:  MULTIBAND  900E  1800 
            版本: 641b09gg.Q2403A 1320676 061804 14:38  
         */
        protected override string[] DeviceMarks => new string[] {
            "WAVECOM MODEM",
            "MULTIBAND  900E  1800",
            "Q2403"
        };
        public override bool SupportCharterSet_UCS2 => false;
        public override SmsDeviceStatus[] Status => base.Status.Concat(new[]
        {
            Status_CSCA
        }).ToArray();

        /// <summary>
        /// 对电话号码进行编码
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        private static string EncodePhone(string phone)
        {
            var result = new StringBuilder();

            /**构建协议头部**/
            result.Append("11");//PDU type (forst octet)文件头字节，一般为11或者01,10为乱码
            result.Append("00");//信息类型（TP-Message-Reference）,一般为00

            /**构建被叫号码地址(目的地址（TP-Destination-Address)**/
            var isInternational = phone.StartsWith("+");
            if (isInternational) phone = phone.Remove(0, 1);//去除前面的+
            var header = (phone.Length << 8) + 0x81 | (isInternational ? 0x10 : 0x20);
            result.Append(Convert.ToString(header, 16).PadLeft(4, '0'));//被叫号码长度+被叫号码类型

            if (phone.Length % 2 == 1) phone = phone + "F";//个数为奇数，则在后面补F凑成偶数
            phone = DataUtils.SwapOddEven(phone);
            result.Append(phone);//互换了奇偶位的电话号码

            /**构建协议尾部**/
            result.Append("00");//协议标识TP-PID,这里一般为00 
            result.Append("08");//数据编码方案TP-DCS（TP-Data-Coding-Scheme）,采用前面说的USC2(16bit)数据编码 
            result.Append("00");//有效期TP-VP（TP-Valid-Period）
            //if (_validityPeriodFormat != ValidityPeriodFormat.FieldNotPresent)
            //    result.Append("00");//有效期TP-VP（TP-Valid-Period）
            //result.Append("A7");//?

            return result.ToString();
        }

        //短信中心号码
        private string smsCenterNumber;

        protected override void InternalSend(string sendTo, string content)
        {
            //清除缓冲区
            ClearBuffer();

            //检查短信中心号码 
            if (string.IsNullOrEmpty(smsCenterNumber))
                smsCenterNumber = Status_CSCA.Read();
            if (string.IsNullOrEmpty(smsCenterNumber))
                throw new ApplicationException("短信中心号码为空！");
            //从短信中心号码得到地区码
            var areaCode = smsCenterNumber.Substring(0, 3);

            //将短信息中心号码去掉+号，看看长度是否为偶数，如果不是，最后添加F  
            var tmp = smsCenterNumber.Substring(1);
            if (tmp.Length % 2 != 0)
                tmp += "F";
            //将奇数位和偶数位交换
            tmp = DataUtils.SwapOddEven(tmp);
            //将短信息中心号码前面加上字符91，91是国际化的意思
            tmp = "91" + tmp;
            //算出 m_strPhoneNo长度，结果除2，格式化成2位的16进制字符串，16 /2 = 8 => "08"
            tmp = "08" + tmp;

            string sendContent = tmp;

            if (!sendTo.StartsWith("+"))
                sendTo = areaCode + sendTo;

            var strSwitch = EncodePhone(sendTo);
            var contentBuffer = EncodeContent(content);

            //加上短信内容长度及短信内容
            strSwitch += (contentBuffer.Length).ToString("X2");
            strSwitch += BitConverter.ToString(contentBuffer).Replace("-", "");
            int nSendLen = strSwitch.Length / 2;
            sendContent += strSwitch;

            string rep;

            if (!WriteCommand("AT+CMGF=0", null, out rep))
                throw new IOException($"发送指令[AT+CMGF=0]时，返回：" + rep);

            WriteLine($"AT+CMGS={nSendLen}");
            rep = ReadLine();
            if (!rep.StartsWith(">"))
                throw new IOException($"发送指令[AT+CMGS={nSendLen}]时，返回：" + rep);

            //发送内容
            Write(Encoding.ASCII.GetBytes(sendContent + (char)0X1A));

            //读取发送到基站报告
            while (true)
            {
                rep = ReadLine(Setting.SmsSendTimeout);
                if (IsERROR(rep))
                    throw new IOException($"发送短信内容后返回：" + rep);
                if (rep.StartsWith("+CMGS:"))
                    break;
            }
            //读取OK行
            ReadLine();
        }
    }
}
