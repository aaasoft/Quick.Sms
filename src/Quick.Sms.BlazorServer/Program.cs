using Quick.Sms.BlazorServer;
using YiQiDong.Agent;

AgentContext.Instance.InitContainerInfo(args);
AgentContext.Instance.InitAgentInstance(new Agent());
AgentContext.Instance.Run();
AgentContext.Instance.Dispose();