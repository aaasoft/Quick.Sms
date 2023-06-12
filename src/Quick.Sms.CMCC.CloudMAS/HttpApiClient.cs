using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quick.Sms.CMCC.CloudMAS
{
    public class HttpApiClient
    {
        private HttpApiClientOptions options;
        public HttpApiClient(HttpApiClientOptions options)
        {
            this.options = options;
        }

        public async Task SendSmsAsync(string mobiles, string content)
        {
            await SendSmsAsync(mobiles, content, CancellationToken.None);
        }

        private class Submit
        {
            public string ecName { get; set; }
            public string apId { get; set; }
            public string mobiles { get; set; }
            public string content { get; set; }
            public string sign { get; set; }
            public string addSerial { get; set; }
            public string mac { get; set; }
        }

        private class ApiResult
        {
            public string rspcod { get; set; }
            public string mgsGroup { get; set; }
            public bool success { get; set; }
        }

        public async Task SendSmsAsync(string mobiles, string content, CancellationToken cancellationToken)
        {
            var sb = new StringBuilder();
            sb.Append(options.ecName);
            sb.Append(options.apId);
            sb.Append(options.secretKey);
            sb.Append(mobiles);
            sb.Append(content);
            sb.Append(options.sign);
            sb.Append(options.addSerial);
            var mac = Md5Utils.Compute(sb.ToString());

            var bodyJson = JsonConvert.SerializeObject(new Submit()
            {
                ecName = options.ecName,
                apId = options.apId,
                mobiles = mobiles,
                content = content,
                sign = options.sign,
                addSerial = options.addSerial,
                mac = mac
            });
            var bodyBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(bodyJson));
            var httpContent = new StringContent(bodyBase64);
            var httpClient = new HttpClient();
            var rep = await httpClient.PostAsync(options.url, httpContent, cancellationToken);
            if (!rep.IsSuccessStatusCode)
                throw new IOException($"{rep.StatusCode} {rep.ReasonPhrase}");
            var repContent = await rep.Content.ReadAsStringAsync();
            var apiResult = JsonConvert.DeserializeObject<ApiResult>(repContent);
            if (apiResult.success)
                return;
            throw new IOException(apiResult.rspcod);
        }
    }
}
