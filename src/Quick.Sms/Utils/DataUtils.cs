using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.Utils
{
    public class DataUtils
    {
        /// <summary>
        /// 互换奇偶位
        /// </summary>
        /// <param name="source">原字符串，如1234567890</param>
        /// <returns>返回互换了奇偶位的字符串，如：2143658709</returns>
        public static string SwapOddEven(string source)
        {
            var result = string.Empty;

            for (var i = 0; i < source.Length; i++)
                result = result.Insert(i % 2 == 0 ? i : i - 1, source[i].ToString());

            return result;
        }
    }
}
