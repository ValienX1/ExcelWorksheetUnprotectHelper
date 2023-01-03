using ExcelHelp.core.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace ExcelHelp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel context)
        {
            InitializeComponent();
            DataContext = context;
            ListView lv = new();
            //lv.SelectedItem
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
