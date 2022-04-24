using Jeopardy.Desktop.Tester.App.Viewmodels;
using System.Windows;

namespace Jeopardy.Desktop.Tester.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewmodel();
        }
    }
}
