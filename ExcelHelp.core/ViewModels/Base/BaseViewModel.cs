using ExcelHelp.core.Models;
using System.ComponentModel;

namespace ExcelHelp.core.ViewModels.Base
{
    /// <summary>
    /// Base View model with static data for all viewmodels
    /// </summary>
    public abstract class BaseViewModel : BaseModel
    {
        #region Private members

        private static string _foregroundColor = string.Empty;
        private static string _hoverForegroundColor = string.Empty;
        private static string _backgroundColor = string.Empty;

        #endregion

        #region Public static properties

        public static string ForegroundColor { get => _foregroundColor; set { _foregroundColor = value; OnStaticPropertyChanged(nameof(ForegroundColor)); } }
        public static string HoverForegroundColor { get => _hoverForegroundColor; set { _hoverForegroundColor = value; OnStaticPropertyChanged(nameof(ForegroundColor)); } }
        public static string BackgroundColor { get => _backgroundColor; set { _backgroundColor = value; OnStaticPropertyChanged(nameof(ForegroundColor)); } }

        #endregion

        #region StaticPropertyChanged Event

        public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged = delegate { };
        private static void OnStaticPropertyChanged(string staticPropertyName)
        {
            StaticPropertyChanged(null, new PropertyChangedEventArgs(staticPropertyName));
        }

        #endregion

        #region Constructor
        public BaseViewModel()
        {
            BackgroundColor = Colors.RaisinBlack;
            ForegroundColor = Colors.KeyLime;
            HoverForegroundColor = Colors.YellowGreenCrayola;
        }
        #endregion
    }
}
