using System;
using System.Collections.Generic;

namespace Quick.Sms.Razor
{
    public static class GeneralExtended
    {
        /// <summary>
        /// 将字符串按指定长度分割成列表
        /// </summary>
        /// <param name="src"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static List<string> Split(this string src, int len)
        {
            List<string> retVal = new List<string>();
            if (string.IsNullOrEmpty(src))
                return retVal;
            int offset = 0;
            do
            {
                string item = src.Substring(offset, Math.Min(src.Length - offset, len));
                retVal.Add(item);
                offset += len;
            } while (offset < src.Length);
            return retVal;
        }

        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static byte[] ToHex(this string src)
        {
            return src.ToHex(0, src == null ? 0 : src.Length);
        }

        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="src"></param>
        /// <param name="index"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static byte[] ToHex(this string src, int index, int len)
        {
            List<byte> retVal = new List<byte>();
            if (string.IsNullOrWhiteSpace(src)
                || src.Length % 2 != 0
                || index < 0
                || len <= 0
                || (index + len) > src.Length)
                return retVal.ToArray();
            src = src.Substring(index, len);
            List<string> hexList = src.Split(2);
            foreach (string item in hexList)
                retVal.Add(Convert.ToByte(item, 16));
            return retVal.ToArray();
        }
    }
}
