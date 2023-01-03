namespace ExcelHelp.core.Models.Item
{
    /// <summary>
    /// Itembase model used in UI
    /// </summary>
    public class ItemBase
    {
        /// <summary>
        /// Name of drive, directory, file, worksheet or error if is encountered
        /// </summary>
        public string? Name { get; set; } = string.Empty;
        /// <summary>
        /// Additional info like protection type of worklsheet
        /// </summary>
        public AdditionalInfo AdditionalInfo { get; set; } = AdditionalInfo.NA;
        /// <summary>
        /// Current item path
        /// </summary>
        public string CurrentPath { get; set; } = string.Empty;
    }
}
