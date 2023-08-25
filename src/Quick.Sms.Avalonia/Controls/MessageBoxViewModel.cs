using Quick.Sms.Avalonia.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quick.Sms.Avalonia.Controls
{
    public class MessageBoxViewModel : ViewModels.PropertyNotifyModel
    {
        private bool _IsVisible = false;
        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                _IsVisible = value;
                RaisePropertyChanged();
            }
        }

        private bool _ButtonOkVisiable = false;
        public bool ButtonOkVisiable
        {
            get { return _ButtonOkVisiable; }
            set
            {
                _ButtonOkVisiable = value;
                RaisePropertyChanged();
            }
        }

        private string _ButtonOkText="OK";
        public string ButtonOkText
        {
            get { return _ButtonOkText; }
            set
            {
                _ButtonOkText = value;
                RaisePropertyChanged();
            }
        }

        private string _Title;
        public string Title
        {
            get { return _Title; }
            set
            {
                _Title = value;
                RaisePropertyChanged();
            }
        }

        private string _Message;
        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand OkCommand { get; set; }

        public MessageBoxViewModel()
        {
            OkCommand = new DelegateCommand() { ExecuteCommand = executeCommand_OkCommand };
        }

        public void Show(string title, string message)
        {
            Title = title;
            Message = message;
            ButtonOkVisiable = true;
            IsVisible = true;
        }

        public void Loading(string title, string message)
        {
            Title = title;
            Message = message;
            ButtonOkVisiable = false;
            IsVisible = true;
        }

        public void Close()
        {
            IsVisible = false;
        }

        private void executeCommand_OkCommand(object e)
        {
            Close();
        }
    }
}
