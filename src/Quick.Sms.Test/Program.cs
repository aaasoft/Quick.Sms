using Quick.Sms.Test;
using YiQiDong.Agent;

//��ʼ��������Ϣ
AgentContext.Instance.InitContainerInfo(args);
//��ʼ��Agentʵ��
AgentContext.Instance.InitAgentInstance(new Agent());
//��ʼ����
AgentContext.Instance.Run();
//����
AgentContext.Instance.Dispose();