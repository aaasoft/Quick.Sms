using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmsTool.Helper
{
    public static class GeneralExtended
    {
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

        /// <summary>
        /// 将16进制字符串转换为字节数组
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Byte[] ToHex(this String src)
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
    }
}
