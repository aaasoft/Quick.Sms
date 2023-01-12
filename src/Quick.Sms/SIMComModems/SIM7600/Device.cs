using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quick.Sms.SIMComModems.SIM7600
{
    public class Device : AbstractSerialPortModem
    {
        /*
            SIMCom_SIM7600系列芯片
            -----------------------------
            生产商: SIMCOM INCORPORATED
            型号: SIMCOM_SIM7600CE-L
            版本: LE11B03SIM7600M11
         */
        protected override string[] DeviceMarks => new[] {
            "SIMCOM","SIM7600"
        };

        public override string Name => "SIMCom_SIM7600系列芯片";
        public SmsDeviceStatus Status_CPSI { get; private set; }
        public SmsDeviceStatus Status_CNSMOD { get; private set; }
        public override SmsDeviceStatus[] Status => base.Status.Concat(new[]
        {
            Status_CSCA,
            Status_CPSI,
            Status_CNSMOD
        }).ToArray();

        public Device()
        {
            Status_CPSI = new SmsDeviceStatus() { Name = "网络信息", Read = () => ExecuteCommand("AT+CPSI?", "+CPSI:") };
            Status_CNSMOD = new SmsDeviceStatus()
            {
                Name = "网络模式",
                Read = () =>
                {
                    var line = ExecuteCommand("AT+CNSMOD?", "+CNSMOD:");
                    if (IsERROR(line))
                        return line;
                    var strs = line.Split(',');
                    if (strs.Length < 2)
                        return line;
                    string mode = strs[1];
                    switch (mode)
                    {
                        case "0":
                            mode = "无服务";
                            break;
                        case "1":
                            mode = "GSM";
                            break;
                        case "2":
                            mode = "GPRS";
                            break;
                        case "3":
                            mode = "EGPRS (EDGE)";
                            break;
                        case "4":
                            mode = "WCDMA";
                            break;
                        case "5":
                            mode = "HSDPA only(WCDMA)";
                            break;
                        case "6":
                            mode = "HSUPA only(WCDMA)";
                            break;
                        case "7":
                            mode = "HSPA (HSDPA and HSUPA, WCDMA)";
                            break;
                        case "8":
                            mode = "LTE";
                            break;
                        case "9":
                            mode = "TDS-CDMA";
                            break;
                        case "10":
                            mode = "TDS-HSDPA only";
                            break;
                        case "11":
                            mode = "TDS-HSUPA only";
                            break;
                        case "12":
                            mode = "TDS-HSPA (HSDPA and HSUPA)";
                            break;
                        case "13":
                            mode = "CDMA";
                            break;
                        case "14":
                            mode = "EVDO";
                            break;
                        case "15":
                            mode = "HYBRID (CDMA and EVDO)";
                            break;
                        case "16":
                            mode = "1XLTE(CDMA and LTE)";
                            break;
                        default:
                            mode = line;
                            break;
                    }
                    return mode;
                }
            };
        }

        public override void InitModem()
        {
            base.InitModem();

            String rep;
            String writeCommand;

            writeCommand = "AT+CMGF=1";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            //检测是否支持AT+CSMP指令，则设置文本模式。(CDMA/EVDO模式不支持AT+CSMP指令)
            writeCommand = "AT+CSMP=?";
            if (WriteCommand(writeCommand, null, out rep))
            {
                writeCommand = "AT+CSMP=17,167,2,25";
                if (!WriteCommand(writeCommand, null, out rep))
                    throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");
            }

            writeCommand = "AT+CSCS=\"UCS2\"";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");
        }

        protected override void InternalSend(string sendTo, string content)
        {
            //清除缓冲区
            ClearBuffer();

            String rep;
            String writeCommand;

            //发送号码
            var hexByteStr = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(sendTo)).Replace("-", String.Empty);
            writeCommand = $"AT+CMGS=\"{hexByteStr}\"";
            WriteLine(writeCommand);
            rep = ReadLine();
            if (!rep.StartsWith(">"))
                throw new IOException($"发送指令[{writeCommand}]时，返回：" + rep);
            //发送内容
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
