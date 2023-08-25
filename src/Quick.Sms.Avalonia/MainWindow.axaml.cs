using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System.Threading.Tasks;

namespace Quick.Sms.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        var viewModel = new ViewModels.MainWindowViewModel();
        DataContext = viewModel;
        Title = viewModel.Title;
        InitializeComponent();
        txtLogs.TextChanged += TxtLogs_TextChanged;
    }

    private void TxtLogs_TextChanged(object sender, TextChangedEventArgs e)
    {
        txtLogs.CaretIndex = int.MaxValue;
    }
}