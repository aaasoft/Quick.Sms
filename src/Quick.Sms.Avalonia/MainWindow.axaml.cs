using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Quick.Sms.Avalonia;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        var viewModel = new ViewModels.MainWindowViewModel();
        DataContext = viewModel;
        Title = viewModel.Title;
        InitializeComponent();
    }
}