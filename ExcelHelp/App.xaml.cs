using ExcelHelp.Extensons;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

namespace ExcelHelp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static ServiceProvider? _serviceProvider;
        public App()
        {
            ServiceCollection services = new();
            services.AddControls();
            services.AddViewModels();
            services.AddServices();
            _serviceProvider = services.BuildServiceProvider();
            string s = "123456789";
            Debug.WriteLine(new string(s.Take(11).ToArray()));
        }
        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow? mainWindow = _serviceProvider!.GetService<MainWindow>();
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement),
                new FrameworkPropertyMetadata(XmlLanguage.GetLanguage(System.Globalization.CultureInfo.CurrentCulture.IetfLanguageTag)));
            mainWindow!.Show();
        }
    }
}
