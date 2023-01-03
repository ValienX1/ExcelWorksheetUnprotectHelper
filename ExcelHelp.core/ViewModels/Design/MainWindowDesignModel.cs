using ExcelHelp.core.Models.Item;

namespace ExcelHelp.core.ViewModels.Design
{
    /// <summary>
    /// Used in design time
    /// </summary>
    public class MainWindowDesignModel : MainWindowViewModel
    {
        public MainWindowDesignModel() : base(null!)
        {
            CurrentDirChildren = new()
            {
                new Drive(){Name = "C:\\", AdditionalInfo = Models.AdditionalInfo.NA},
                new Drive(){Name = "D:\\", AdditionalInfo = Models.AdditionalInfo.NA},
                new Drive(){Name = "E:\\", AdditionalInfo = Models.AdditionalInfo.NA},
                new Drive(){Name = "F:\\", AdditionalInfo = Models.AdditionalInfo.NA},
            };
        }
    }
}
