using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.WavecomModems.CDMA
{
    public class Device : AbstractSerialPortModem
    {
        //内容尾巴
        private byte[] contentTail = new byte[] { 0x00, 0x1a };
        public override bool SupportResponseHead => true;
        public override string Name => "Wavecom_CDMA芯片";
        /*
            Wavecom CDMA芯片
            -----------------------------
            生产商: ERROR
            型号: ERROR
            版本: S/W VER: WISMOQ        WQ2.17R Jul 13 2005 09:02:14
         */
        protected override string[] DeviceMarks => new[] {
            "WISMOQ",
            "WQ2.17R"
        };

        public override SmsDeviceStatus[] Status => base.Status.Concat(
            new SmsDeviceStatus[]
            {
                new SmsDeviceStatus()
                {
                    Name = "字符集",
                    Read = ()=>ExecuteCommand("AT+CSCS=?", "+CSCS:")
                },
                new SmsDeviceStatus()
                {
                    Name = "IMSI",
                    Read = ()=>ExecuteCommand("AT+CIMI", "+CIMI:")
                },
                new SmsDeviceStatus()
                {
                    Name = "计时",
                    Read=()=>
                    {
                        var line =ExecuteCommand("AT+WTMR", "+WTMR:");
                        if(IsERROR(line))
                            return line;
                        var strs = line.Split(',');
                        if(strs.Length<3)
                            return line;
                        return $"开机[{TimeSpan.FromSeconds(int.Parse(strs[0]))}],通话[总时长:{strs[1]}秒,总数量:{strs[2]}]";
                    }
                }
            }).ToArray();

        protected override void InternalSend(string sendTo, string content)
        {
            //确保短信猫工作正常
            EnsureModem();

            //清除缓冲区
            ClearBuffer();

            string rep;
            if (!WriteCommand("AT+WSCL=6,4", null, out rep))
                throw new IOException($"发送指令[{"AT+WSCL=6,4"}]时，响应：{rep}");

            //发送内容
            var contentBuffer = EncodeContent(content);
            //电话号码与内容长度指令
            var commandBuffer = Encoding.Default.GetBytes($"AT+CMGS=\"{sendTo }\",{contentBuffer.Length}{WriteNewLine}");

            //推送短信内容
            if (!WriteCommand(commandBuffer.Concat(contentBuffer).Concat(contentTail).ToArray(), "+CMGS:", out rep))
                throw new IOException("发送短信时，返回：" + rep);
            //读取发送到基站报告
            while (true)
            {
                rep = ReadLine(Setting.SmsSendTimeout);
                if (rep.StartsWith("+CDS:"))
                    break;
            }
        }
    }
}
