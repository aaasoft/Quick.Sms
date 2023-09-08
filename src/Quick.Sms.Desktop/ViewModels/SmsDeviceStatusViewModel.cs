using Quick.Sms.Desktop.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.Desktop.ViewModels
{
    public class SmsDeviceStatusViewModel : PropertyNotifyModel
    {
        private MessageBoxViewModel MessageBox;
        public DelegateCommand ReadStatusCommand { get; set; }
        public DelegateCommand WriteStatusCommand { get; set; }

        private SmsDeviceStatus _Status;
        public SmsDeviceStatus Status
        {
            get { return _Status; }
            set
            {
                _Status = value;
                RaisePropertyChanged();
            }
        }
        public bool CanWrite => Status.Write != null;

        private string _Value;
        public string Value
        {
            get { return _Value; }
            set
            {
                _Value = value;
                RaisePropertyChanged();
            }
        }

        public SmsDeviceStatusViewModel(MessageBoxViewModel messageBox)
        {
            MessageBox = messageBox;
            ReadStatusCommand = new DelegateCommand() { ExecuteCommand = executeCommand_ReadStatusCommand };
            WriteStatusCommand = new DelegateCommand() { ExecuteCommand = executeCommand_WriteStatusCommand };
        }

        public async Task ReadStatus(bool shouldCloseLoading = true)
        {
            try
            {
                MessageBox.Loading("读取中", $"正在读取[{Status.Name}]的数据...");
                string ret = null;
                await Task.Run(() => ret = Status.Read());
                Value = ret;
            }
            catch (Exception ex)
            {
                Value = $"读取失败，原因：{ex.Message}";
            }
            finally
            {
                if (shouldCloseLoading)
                    MessageBox.Close();
            }
        }

        private async void executeCommand_ReadStatusCommand(object e)
        {
            await ReadStatus(true);
        }

        private void executeCommand_WriteStatusCommand(object e)
        {
            MessageBox.Prompt("输入", $"请输入要设置[{Status.Name}]的值", Value, async content =>
            {
                try
                {
                    await Task.Delay(1);
                    MessageBox.Loading("写入中", $"正在写入[{Status.Name}]的数据...");
                    await Task.Run(() => Status.Write(content));
                    Value = content;
                    MessageBox.Show("成功", $"写入[{Status.Name}]的值成功。");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("失败", $"写入[{Status.Name}]的值失败，原因：{ex.Message}");
                }
            });
        }
    }
}
