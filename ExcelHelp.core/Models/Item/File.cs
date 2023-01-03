namespace ExcelHelp.core.Models.Item
{
    /// <summary>
    /// Model for File
    /// </summary>
    public class File : ItemBase
    {
        /// <summary>
        /// Additional property storring file extension
        /// </summary>
        public string Extension { get; set; } = string.Empty;
    }
}
