using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quick.Protocol;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using YiQiDong.Agent;
using YiQiDong.Core;
using YiQiDong.Core.Utils;
using YiQiDong.Protocol.V1.Model;

namespace Quick.Sms.Test
{
    public class Agent : AbstractAgent
    {
        private CancellationTokenSource cts;
        private WebApplication app;

        public override void Start()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;
            Task.Run(() =>
            {
#if DEBUG
                var webApplicationOptions = new WebApplicationOptions();
#else
            var webApplicationOptions = new WebApplicationOptions()
            {
                ContentRootPath = System.IO.Path.GetDirectoryName(typeof(Agent).Assembly.Location)
            };
#endif
                var builder = WebApplication.CreateBuilder(webApplicationOptions);
#if DEBUG
                builder.WebHost.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
#endif
                builder.Logging.ClearProviders();
                builder.Services.AddRazorPages();
                builder.Services.AddServerSideBlazor()
                    .AddCircuitOptions(options => { options.DetailedErrors = true; });
                builder.WebHost.UseUrls("http://127.0.0.1:0");
                app = builder.Build();
                if (builder.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Error");
                }

                app.UseStaticFiles();
                app.UseRouting();
                app.MapBlazorHub();
                app.MapFallbackToPage("/_Host");

                while (true)
                {
                    if (token.IsCancellationRequested)
                        return;
                    //启动Web服务
                    try
                    {
                        app.Start();
                        AgentContext.Instance.Client?.SendCommand(new YiQiDong.Protocol.V1.QpCommands.AddReverseProxyRule.Request()
                        {
                            Name = "访问",
                            Path = "/",
                            Url = app.Urls.First()
                        })?.ContinueWith(task =>
                        {
                            if (task.IsFaulted)
                                AgentContext.Instance.LogWarn($"注册反向代理规则失败，原因：{ExceptionUtils.GetExceptionString(task.Exception.InnerException)}");
                            else
                                AgentContext.Instance.LogInfo($"注册反向代理规则成功");
                        });
                        AgentContext.Instance.LogInfo($"Web服务启动完成，地址：{string.Join(",", app.Urls)}");
                        break;
                    }
                    catch (Exception ex)
                    {
                        var message = $"启动Web服务时失败，原因：" + ex.Message;
                        AgentContext.Instance.LogError(message);
                        if (ContainerInfo == null)
                            throw new Exception(message, ex);
                        Thread.Sleep(5000);
                    }
                }
            });
        }

        public override void Stop()
        {
            cts?.Cancel();
            cts = null;

            app?.StopAsync();
            app = null;
        }
    }
}
