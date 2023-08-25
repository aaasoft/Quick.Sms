using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using System.Threading.Tasks;

namespace Quick.Sms.Avalonia;

public partial class MainWindow : Window
{
    private ScrollViewer txtLogsScrollViewer;
    public MainWindow()
    {
        var viewModel = new ViewModels.MainWindowViewModel();
        DataContext = viewModel;
        Title = viewModel.Title;
        InitializeComponent();
        txtLogs.TextChanged += TxtLogs_TextChanged;
        txtLogsScrollViewer = FindVisualTreeChild<ScrollViewer>(txtLogs);
    }

    private T FindVisualTreeChild<T>(Visual visual) where T : Visual
    {
        foreach (var child in visual.GetVisualChildren())
        {
            if (visual is T t)
                return t;
            var childT = FindVisualTreeChild<T>(child);
            if (childT != null)
                return childT;
        }
        return null;
    }

    private void TxtLogs_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (txtLogsScrollViewer == null)
            txtLogsScrollViewer = FindVisualTreeChild<ScrollViewer>(txtLogs);
        txtLogsScrollViewer?.ScrollToEnd();
    }
}