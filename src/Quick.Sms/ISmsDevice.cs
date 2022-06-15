using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms
{
    public interface ISmsDevice
    {
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 设置类型
        /// </summary>
        Type SettingType { get; }
        /// <summary>
        /// 获取功能
        /// </summary>
        SmsDeviceFeature[] Features { get; }
        /// <summary>
        /// 获取状态
        /// </summary>
        SmsDeviceStatus[] Status { get; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="settingObj">设置对象</param>
        void Init(Object settingObj);
        /// <summary>
        /// 打开设备
        /// </summary>
        void Open();
        /// <summary>
        /// 检查是否匹配
        /// </summary>
        void CheckIsMatch();
        /// <summary>
        /// 关闭设备
        /// </summary>
        void Close();
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="sendTo">发送到</param>
        /// <param name="content">发送内容</param>
        void Send(string sendTo, string content);
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="bytesToWrite"></param>
        void ExecuteCommand(byte[] bytesToWrite);
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        void ExecuteCommand(string command);
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command">命令</param>
        /// <returns></returns>
        string ExecuteCommand(string command, string responseHead);
        /// <summary>
        /// 发送了一行数据事件
        /// </summary>
        event EventHandler<string> LineSended;
        /// <summary>
        /// 接收到一行数据事件
        /// </summary>
        event EventHandler<string> LineRecved;
        /// <summary>
        /// 警告事件
        /// </summary>
        event EventHandler<string> Warning;
    }
}
