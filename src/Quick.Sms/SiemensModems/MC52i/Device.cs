using Quick.Sms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Quick.Sms.SiemensModems.MC52i
{
    public class Device : AbstractSerialPortModem
    {
        /*
    Siemens_MC52I系列芯片
    -----------------------------
    生产商: OK
    型号: MC52i
    版本: REVISION 01.200
 */
        protected override string[] DeviceMarks => new[] {
            "OK","MC52i"
        };

        public override string Name => "Siemens_MC52i系列芯片";

        public override void InitModem()
        {
            base.InitModem();

            string rep;
            string writeCommand = "AT+CMGF=1";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            writeCommand = "AT+CSCS=\"UCS2\"";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            writeCommand = "AT+CSMP=17,167,0,8";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");
        }

        protected override void InternalSend(string sendTo, string content)
        {
            //清除缓冲区
            ClearBuffer();

            string rep;
            string writeCommand;

            var hexByteStr = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(sendTo)).Replace("-", String.Empty);
            writeCommand = $"AT+CMGS=\"{hexByteStr}\"";
            WriteLine(writeCommand);
            rep = ReadLine();
            if (!rep.StartsWith(">"))
                throw new IOException($"发送指令[{writeCommand}]时，返回：" + rep);

            hexByteStr = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(content)).Replace("-", String.Empty);
            writeCommand = $"{hexByteStr}{(char)0x1A}";

            Write(Encoding.ASCII.GetBytes(writeCommand));

            //读取CMGS返回
            while (true)
            {
                rep = ReadLine(Setting.SmsSendTimeout);
                if (rep.StartsWith("+CMS ERROR:"))
                    throw new IOException(rep);
                if (rep.StartsWith("+CMGS:"))
                    break;
            }
        }
    }
}
