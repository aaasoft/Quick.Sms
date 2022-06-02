using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmsTool
{
    public partial class SendStatusForm : Form
    {
        public SendStatusForm()
        {
            InitializeComponent();
        }

        public void PushLog(string log)
        {
            Invoke(new Action(() =>
            {
                txtLogs.AppendText($"{DateTime.Now.ToString("HH:mm:ss")}: {log}{Environment.NewLine}");
            }));
        }

        public void PushProcess(int current)
        {
            Invoke(new Action(() => pbSend.Value = current));
        }

        public void SetMaximum(int maximum)
        {
            Invoke(new Action(() => pbSend.Maximum = maximum));
        }
    }
}
