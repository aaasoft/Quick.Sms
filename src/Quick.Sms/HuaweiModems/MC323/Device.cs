using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.HuaweiModems.MC323
{
    public class Device : AbstractSerialPortModem
    {
        //内容尾巴
        private byte[] contentTail = new byte[] { 0x00, 0x1a };
        /*
            华为MC323-a芯片
            -----------------------------
            生产商: HUAWEI TECHNOLOGIES CO., LTD
            型号: MC323-a
            版本: 12.102.21.00.000
         */
        protected override string[] DeviceMarks => new[]{
            "HUAWEI",
            "MC323"
        };

        public override string Name => "华为_MC323系列芯片";


        public override SmsDeviceStatus[] Status => base.Status.Concat(new[]
        {
            new SmsDeviceStatus(){Name="硬件版本",Read=()=>ExecuteCommand("AT^HWVER","^HWVER:") },
            new SmsDeviceStatus(){Name="MEID",Read=()=>ExecuteCommand("AT^MEID","") },
            new SmsDeviceStatus(){Name="IMSI",Read = ()=>ExecuteCommand("AT+CIMI", "") },
            new SmsDeviceStatus(){Name="ICCID",Read=()=>ExecuteCommand("AT+CCID","+CCID:") },
            new SmsDeviceStatus(){Name="基站信息",Read=()=>ExecuteCommand("AT^BSINFO","^BSINFO:") },
        }).ToArray();

        protected override void InternalSend(string sendTo, string content)
        {
            //清除缓冲区
            ClearBuffer();

            string rep;
            if (!WriteCommand("at^HSMSSS=0,0,6,0", null, out rep))
                throw new IOException("发送指令[at^HSMSSS=0,0,6,0]，返回：" + rep);
            if (!WriteCommand("AT+CMGF=1", null, out rep))
                throw new IOException("发送指令[AT+CMGF=1]，返回：" + rep);

            WriteLine($"AT^HCMGS=\"{sendTo}\"");
            rep = ReadLine();
            if (!rep.StartsWith(">"))
                throw new IOException($"发送指令[AT^HCMGS=\"{sendTo}\"]时，返回：" + rep);

            //发送内容
            var contentBuffer = EncodeContent(content);
            //推送短信内容
            if (!WriteCommand(contentBuffer.Concat(contentTail).ToArray()))
                throw new IOException("发送短信时，未返回OK");
            //读取发送到基站报告
            while (true)
            {
                rep = ReadLine(Setting.SmsSendTimeout);
                if (rep.StartsWith("^HCMGSS:"))
                    break;
                if (rep.StartsWith("^HCMGSF:"))
                    throw new IOException("发送短信时，返回失败，响应：" + rep);
            }
        }
    }
}
