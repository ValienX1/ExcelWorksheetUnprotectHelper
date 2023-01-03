using ExcelHelp.core.Services;
using ExcelHelp.core.Services.Interfaces;
using ExcelHelp.core.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace ExcelHelp.Extensons
{
    public static class IServiceCollectionExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            _ = services.AddSingleton<IRepository, Repository>();
        }
        public static void AddControls(this IServiceCollection services)
        {
            _ = services.AddSingleton<MainWindow>();
        }
        public static void AddViewModels(this IServiceCollection services)
        {
            _ = services.AddSingleton<MainWindowViewModel>();
        }
    }
}
