using System.ComponentModel;

namespace ExcelHelp.core.ViewModels.Base
{
    /// <summary>
    /// Base model with property change simplification
    /// </summary>
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };
        public void OnPropertyChanged(string name)
        {
            PropertyChanged!(this, new PropertyChangedEventArgs(name));
        }
    }
}
