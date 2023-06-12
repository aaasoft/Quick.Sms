using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.CMCC.CloudMAS
{
    public class HttpApiClientOptions
    {
        public string url { get; set; }
        public string ecName { get; set; }
        public string apId { get; set; }
        public string secretKey { get; set; }
        public string sign { get; set; }
        public string addSerial { get; set; }
    }
}
