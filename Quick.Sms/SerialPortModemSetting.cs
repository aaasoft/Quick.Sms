namespace Quick.Sms;

public class SerialPortModemSetting
{
    static SerialPortModemSetting()
    {
        UrlClient.SerialPort.SerialPortUrlClient.Register();
        UrlClient.Tcp.TcpUrlClient.Register();
    }

    /// <summary>
    /// 连接URL
    /// </summary>
    public string Url { get; set; }
    /// <summary>
    /// 读取响应超时
    /// </summary>
    public int ReadResponseTimeout { get; set; } = 10 * 1000;
    /// <summary>
    /// 短信发送超时时间
    /// </summary>
    public virtual int SmsSendTimeout { get; set; } = 60 * 1000;
}