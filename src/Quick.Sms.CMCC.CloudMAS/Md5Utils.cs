using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.CMCC.CloudMAS
{
    internal class Md5Utils
    {
        public static string Compute(string input)
        {
#if NET6_0_OR_GREATER
            var buffer = MD5.HashData(Encoding.UTF8.GetBytes(input));            
#else
            var buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
#endif
            return BitConverter.ToString(buffer).Replace("-", "").ToLower();
        }
    }
}
