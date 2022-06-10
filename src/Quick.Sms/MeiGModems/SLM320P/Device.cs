using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quick.Sms.MeiGModems.SLM320P
{
    public class Device : AbstractSerialPortModem
    {
        /*
            SLM320P_MeiG系列芯片
            -----------------------------
            生产商: 崇瀚
            型号: MeiG
            版本: REVISION 01.200
        */
        protected override string[] DeviceMarks => new[] {
            "MeiG", "SLM320P"
        };

        public override string Name => "崇瀚_SLM320P系列芯片";

        public override bool SliceLongText => false;

        protected override void InternalSend(string sendTo, string content)
        {
            //确保短信猫工作正常
            EnsureModem();

            //清除缓冲区
            ClearBuffer();

            String rep;
            String writeCommand;

            writeCommand = "AT+SETVOLTE=1";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            writeCommand = "AT+CSCS=\"UCS2\"";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            writeCommand = "AT+CMGF=0";
            if (!WriteCommand(writeCommand, null, out rep))
                throw new IOException($"发送指令[{writeCommand}]时，响应：{rep}");

            var groupId = GetPackageId();
            var contents = content.Split(60);
            var count = contents.Count;
            var index = 1;
            sendTo = $"{sendTo}f";
            sendTo = String.Join(String.Empty, sendTo.Split(2).Select(s => new String(s.Reverse().ToArray())));
            foreach (var text in contents)
            {
                var contentHex = BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(text)).Replace("-", String.Empty);
                var len = (Byte)(contentHex.Length / 2 + 6);
                var lenHex = BitConverter.ToString(new Byte[] { len });
                var encodingTxt = $"0051000b81{sendTo}0008a7{lenHex}050003{groupId}{count:x2}{index++:x2}{contentHex}{(char)0x1a}";
                var contentLen = ((encodingTxt.Length - 1) / 2) - 1; //去掉最后0x1a与开头的"00"的长度

                writeCommand = $"AT+CMGS={contentLen}";
                WriteLine(writeCommand);

                rep = ReadLine();
                if (!rep.StartsWith(">"))
                    throw new IOException($"发送指令[{writeCommand}]时，返回：" + rep);

                writeCommand = encodingTxt.ToLower();
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
                Thread.Sleep(500);
                //清除缓冲区
                ClearBuffer();
            }
        }

        private readonly static Int64 _packageIdMode = ((Int64)Byte.MaxValue + 1L);
        private String GetPackageId()
        {
            var baseDir = Path.GetDirectoryName(this.GetType().Assembly.Location);
            var file = Path.Combine(baseDir, @"GlobalPackageId.data");
            var _globalPackageId = 0L;
            try
            {
                if (File.Exists(file))
                {
                    var txt = File.ReadAllText(file);
                    Int64.TryParse(txt, out _globalPackageId);
                }
            }
            catch { }
            var newPackageId = Interlocked.Increment(ref _globalPackageId);
            try { File.WriteAllText(file, newPackageId.ToString()); } catch { }
            return $"{(Byte)(newPackageId % _packageIdMode):x2}";
        }
    }

    public static class GeneralExtended
    {
        #region 将字符串按指定长度分割成列表
        /// <summary>
        /// 将字符串按指定长度分割成列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static List<String> Split(this String src, Int32 len)
        {
            List<String> retVal = new List<String>();
            if (String.IsNullOrEmpty(src))
                return retVal;
            Int32 offset = 0;
            do
            {
                String item = src.Substring(offset, Math.Min(src.Length - offset, len));
                retVal.Add(item);
                offset += len;
            } while (offset < src.Length);
            return retVal;
        }
        #endregion

        #region 将16进制字符串转换为字节数组
        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Byte[] ToHex(this String src)
        {
            return src.ToHex(0, src == null ? 0 : src.Length);
        }
        #endregion

        #region 将16进制字符串转换为字节数组
        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static Byte[] ToHex(this String src, Int32 index, Int32 len)
        {
            List<Byte> retVal = new List<Byte>();
            if (String.IsNullOrWhiteSpace(src)
                || src.Length % 2 != 0
                || index < 0
                || len <= 0
                || (index + len) > src.Length)
                return retVal.ToArray();
            src = src.Substring(index, len);
            List<String> hexList = src.Split(2);
            foreach (String item in hexList)
                retVal.Add(Convert.ToByte(item, 16));
            return retVal.ToArray();
        }
        #endregion
    }
}
