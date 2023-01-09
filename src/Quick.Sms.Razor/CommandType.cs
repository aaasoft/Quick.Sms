using System.ComponentModel;

namespace Quick.Sms.Razor
{
    public enum CommandType
    {
        Text,
        Hex
    }

    public class CommandTypeStringConverter
    {
        public static string ToString(CommandType item)
        {
            switch (item)
            {
                case CommandType.Text:
                    return "文本";
                case CommandType.Hex:
                    return "十六进制";
                default:
                    return item.ToString();
            }
        }
    }
}
